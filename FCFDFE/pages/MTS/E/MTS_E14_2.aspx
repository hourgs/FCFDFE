<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_E14_2.aspx.cs" Inherits="FCFDFE.pages.MTS.E.MTS_E14_2" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <asp:Panel ID="Panel_BudgetInfNo" runat="server" Visible="false">
                <section class="panel">
                <header  class="title">
                    運費通知單查詢
                </header>
                <asp:Panel ID="Panel2" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:100px;" class="td_subtitle">
                                        <asp:Label CssClass="control-label" runat="server">年度</asp:Label>
                                    </td>
                                    <td style="width:200px;">
                                        <asp:DropDownList ID="Year" CssClass="tb tb-m" runat="server">
                                        </asp:DropDownList>
                                    </td>        
                                     <td style="width:100px;" class="td_subtitle">
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td style="width:200px;">
                                        <asp:DropDownList ID="Mil_type" CssClass="tb tb-m" runat="server">
                                        </asp:DropDownList>
                                    </td>                  
                                     <td style="width:100px;" class="td_subtitle">
                                        <asp:Label CssClass="control-label" runat="server">費用別</asp:Label>
                                    </td>
                                    <td style="width:200px;">
                                        <asp:DropDownList ID="qExpense_TYPE" CssClass="dropdownlist" runat="server">
                                            <asp:ListItem Value="運費">運費</asp:ListItem>
                                                <asp:ListItem Value="保費">保費</asp:ListItem>
                                                <asp:ListItem Value="在美(歐)運費">在美(歐)運費</asp:ListItem>
                                                <asp:ListItem Value="差旅費">差旅費</asp:ListItem>
                                                <asp:ListItem Value="物品費">物品費</asp:ListItem>
                                                <asp:ListItem Value="通訊費">通訊費</asp:ListItem>
                                           </asp:DropDownList>
                                    </td>                  
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnQuery" cssclass="btn-success" runat="server" Text="查詢" OnClick="btnQuery_Click"/> <br /><br />
                            </div>
                            <asp:GridView ID="GV" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_PreRender" OnRowCommand="GV_RowCommand" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="OVC_BUD_NO" HeaderText="運算通知單編號" />
                                    <asp:BoundField DataField="OVC_MILITARY_TYPE" HeaderText="軍種" />
                                    <asp:BoundField DataField="OVC_EXPENSE_TYPE" HeaderText="費用別" />
                                    <asp:BoundField DataField="OVC_BUDGET_TYPE" HeaderText="預算科目及編號" />
                                    <asp:BoundField DataField="OVC_PURPOSE_TYPE" HeaderText="用途別" />
                                    <asp:BoundField DataField="ONB_BUD_AMOUNT" HeaderText="預算金額" />
                                    <asp:BoundField DataField="REMAIN" HeaderText="預算餘額" />
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnChoose" CssClass="btn-warning" Text="選取" CommandName="btnChoose" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="Panel_main" runat="server">
            <section class="panel">
                <header  class="title">
                    運費 結報申請表-修改
                </header>
                <div class="text-right" style="width:100%">
                     <asp:Button ID="btnReturn" cssclass="btn-success"  runat="server" Text="回運費結報申請表管理" OnClick="btnReturn_Click"/>&nbsp;&nbsp;  
                </div>
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
                                        <asp:Label ID="lblOvcInfNo" CssClass="control-label" runat="server" Text=""></asp:Label>
                                    </td>                                   
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">案由</asp:Label>
                                    </td>
                                    <td class="no-bordered" style="width:225px;">
                                        <asp:DropDownList ID="drpOvcGist" CssClass="tb tb-m" runat="server">
                                           
                                        </asp:DropDownList>        
                                    </td>
                                    <td class="no-bordered" style="width:575px;"colspan="4">
                                        <asp:TextBox ID="txtOvcGist" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">預算科目及編號</asp:Label>
                                    </td>
                                    <td style="width:225px;">
                                        <asp:DropDownList ID="drpOvcBudgetInfNo" CssClass="tb tb-m" runat="server">
                                            <asp:ListItem Text="未輸入" Value="未輸入"></asp:ListItem>
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
                                           <asp:ListItem Text="海運" Value="海運"></asp:ListItem>
                                            <asp:ListItem Text="空運" Value="空運"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:125px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">進出口別</asp:Label>
                                    </td>
                                    <td style="width:150px;">
                                        <asp:DropDownList ID="drpOvcImpOrExp" CssClass="tb tb-s" runat="server">
                                           <asp:ListItem Text="進口" Value="進口"></asp:ListItem>
                                            <asp:ListItem Text="出口" Value="出口"></asp:ListItem>
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
                                        <asp:Label CssClass="control-label" runat="server">元</asp:Label>&nbsp;&nbsp;                                    </td>                                   
                                </tr>
                                 <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">預算通知單編號</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="5">
                                        <asp:TextBox ID="txtOvcBudgetInfNo" CssClass="tb tb-l" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Button ID="btnLoad" cssclass="btn-success" runat="server" Text="載入預算通知單編號" OnClick="btnLoad_Click"/>&nbsp;&nbsp;  
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
                                        </asp:ListBox>
                                    </td>                                   
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">擬辦</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="5">
                                        <asp:ListBox ID="lstOvcPlnContent" CssClass="tb tb-l"  runat="server">                                          
                                        </asp:ListBox>
                                    </td>                                   
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSave" cssclass="btn-warning" runat="server" Text="修改結報申請表" OnClick="btnSave_Click" /> 
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
                                        <asp:DropDownList ID="drpOvcBldNo" CssClass="tb tb-s" runat="server">
                                             <asp:ListItem>107</asp:ListItem>
                                            <asp:ListItem>106</asp:ListItem>
                                            <asp:ListItem>105</asp:ListItem>
                                            <asp:ListItem>104</asp:ListItem>
                                            <asp:ListItem>103</asp:ListItem>
                                            <asp:ListItem>102</asp:ListItem>
                                            <asp:ListItem>101</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                            <asp:ListItem>99</asp:ListItem>
                                            <asp:ListItem>98</asp:ListItem>
                                            <asp:ListItem>97</asp:ListItem>
                                            <asp:ListItem>96</asp:ListItem>
                                            <asp:ListItem>95</asp:ListItem>
                                            <asp:ListItem>94</asp:ListItem>
                                            <asp:ListItem>93</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label CssClass="control-label"  runat="server">年</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label"  runat="server">提單編號</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox ID="txBldfNo" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnFilter" CssClass="btn-success btnw2" runat="server" Text="過濾" OnClick="btnFilter_Click" />
                                    </td>
                                    <td style="text-align:center;vertical-align:middle;">
                                        <asp:LinkButton ID="linkAll" runat="server" ForeColor="#6699FF" OnClick="linkAll_Click">全部勾選</asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton ID="linkCancel" runat="server" ForeColor="#FFCC99" OnClick="linkCancel_Click">全部取消</asp:LinkButton>
                                    </td>
                                </tr>
                                 <tr>
                                    <td  colspan="6">
                                        <asp:CheckBoxList ID="chkOvcInfNo" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Table" runat="server" RepeatColumns="5">
                                            
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnAdd" cssclass="btn-warning" runat="server" Text="↓加入運費" OnClick="btnAdd_Click" /><br /><br /> 
                            </div>
                            <asp:GridView ID="GridViewTBGMT_CINF" DataKeyNames="OVC_BLD_NO" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GridViewTBGMT_CINF_PreRender"  OnRowCommand="GridViewTBGMT_CINF_RowCommand" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="OVC_ICS_NO" HeaderText="運費編號" />
                                    <asp:TemplateField HeaderText="提單案號" >
                                    <ItemTemplate>
                                                    <a ID="link" href="javascript:var win=window.open('BLDDATA.aspx?OVC_BLD_NO=<%# Eval("OVC_BLD_NO")%>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=700,left=0,top=0');" >
                                                        <%# Eval("OVC_BLD_NO")%></a>
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="OVC_INLAND_CARRIAGE" HeaderText="海空運費" />
                                    <asp:BoundField DataField="OVC_INF_NO" HeaderText="捷報申請表編號" />
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnDel" CssClass="btn-danger" Text="刪除" CommandName="btnDel" runat="server"/>
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
          </asp:Panel>
        </div>
    </div>
</asp:Content>
