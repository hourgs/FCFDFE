<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_C13.aspx.cs" Inherits="FCFDFE.pages.MPMS.C.MPMS_C13" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                    <!--標題-->計劃評核－承辦採購計劃查詢
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="subtitle">查詢條件(選擇年度後，擇一來做查詢條件)</div>
                            <table class="table table-bordered" style="text-align:center">
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">計劃年度(第二組)： </asp:Label>
                                        <asp:DropDownList ID="drpOVC_BUDGET_YEAR" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>106</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Button ID="btnQuerynoONB" CssClass="btn-success" CommandName="btnQuerynoONB" OnCommand="btnQuery_Command" runat="server" Text="尚未移辦採包查詢" /><!--綠色-->
                                        <asp:Button ID="btnQuerymyOVC" CssClass="btn-success" CommandName="btnQuerymyOVC" OnCommand="btnQuery_Command" runat="server" Text="顯示個人承辦案件統計明細" /><!--綠色-->
                                        <asp:Button ID="btnQueryallOVC" CssClass="btn-success btnw4" CommandName="btnQueryallOVC" OnCommand="btnQuery_Command" runat="server" Text="顯示全部" /><!--綠色-->
                                    </td>
                                </tr>
                            </table>
                                
                            <div class="subtitle">查詢結果</div>
                            <asp:GridView ID="GV_Query_PLAN" CssClass=" table data-table table-striped border-top"
                                            AutoGenerateColumns="false" OnPreRender="GV_Query_PLAN_PreRender" OnRowDataBound="GV_Query_PLAN_RowDataBound"  runat="server">
                                <Columns>
                                     <asp:TemplateField HeaderText="購案編號" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnPurch" runat="server" Text ='<%# "" +Eval("OVC_PURCH") + Eval("OVC_PUR_AGENCY")%>'
                                                OnClientClick='<%# Eval("OVC_PURCH","javascript:window.open(\"TRANSFORM_C.aspx?OVC_PURCH={0}\",null,\"scrollbars=yes, toolbar=0,location=0,status=0,menubar=0,width=700,height=500,left=200,top=80\");")%>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="購案名稱" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnPurchName" runat="server" Text ='<%#Eval("OVC_PUR_IPURCH")%>'
                                                OnClientClick='<%# Eval("OVC_PURCH","javascript:window.open(\"STATUS.aspx?OVC_PURCH={0}\",null,\"scrollbars=yes, toolbar=0,location=0,status=0,menubar=0,width=700,height=500,left=200,top=80\");")%>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:BoundField HeaderText="委購單位" DataField="OVC_PUR_NSECTION" />
                                    <asp:BoundField HeaderText="審查總次數" DataField="ONB_CHECK_TIMES" />
                                    <asp:BoundField HeaderText="最近分派日期" DataField="MaxDate" />
                                    <asp:TemplateField ItemStyle-CssClass="text-center" HeaderText="逾一個月未核定" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblSignMonth" runat="server" Visible="false">
                                                <span class="glyphicon glyphicon-warning-sign" style="color:red"></span>
                                            </asp:Label>
                                            <asp:HiddenField ID="hidMinDate" Value='<%#Bind("MinDate")%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField ItemStyle-CssClass="text-center" HeaderText="申購單位逾初審7日、複審5日時限" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblSignDate" runat="server" Visible="false">
                                                <span class="glyphicon glyphicon-warning-sign" style="color:red"></span>
                                            </asp:Label>
                                            <asp:HiddenField ID="hidOVC_CHECK_OK" Value='<%#Bind("OVC_CHECK_OK")%>' runat="server" />
                                            <asp:HiddenField ID="hidOVC_PERMISSION_UPDATE" Value='<%#Bind("OVC_PERMISSION_UPDATE")%>' runat="server" />
                                            <asp:HiddenField ID="hidOVC_DREJECT" Value='<%#Bind("OVC_DREJECT")%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField ItemStyle-CssClass="text-center" HeaderText="功能" >
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success btnw2" ID="btnToAUDIT" Text="查詢" CommandName="btnToAUDIT"
                                                CommandArgument='<%#Eval("OVC_PURCH")%>' OnCommand="btnToAUDIT_Command" runat="server"/>
                                            <asp:Button CssClass="btn-success btnw" ID="btnCheck" CommandName="btnCheck" Text="查核預算分組" 
                                                CommandArgument='<%#Eval("OVC_PURCH")%>' OnCommand="btnToAUDIT_Command" Visible="false" runat="server"/>
                                            <asp:HiddenField ID="hidIS_PLURAL_BASIS" Value='<%#Bind("IS_PLURAL_BASIS")%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                	            </Columns>
	   		               </asp:GridView>

                            <asp:GridView ID="GV_CASE" ShowFooter="true" CssClass=" table data-table table-striped border-top" AutoGenerateColumns="false" Visible="false" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="作業" />
                                    <asp:BoundField HeaderText="承辦人姓名" DataField="Name" />
                                    <asp:BoundField HeaderText="已承辦案數" DataField="Count" />
                	            </Columns>
	   		               </asp:GridView>
                    </div>
                </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
