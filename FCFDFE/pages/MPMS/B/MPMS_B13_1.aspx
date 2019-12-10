<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_B13_1.aspx.cs" Inherits="FCFDFE.pages.MPMS.B.MPMS_B13_1" %>
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
                    <!--標題-->登錄預算科目
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="text-center">
                                <asp:Label CssClass="control-label" runat="server" Text="預算來源("></asp:Label>
                                <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                                <asp:Label CssClass="control-label" runat="server" Text=")"></asp:Label>
                            </div>
                            <asp:GridView ID="GV_LoginBudget" CssClass="table table-striped border-top text-center" OnPreRender="GV_LoginBudget_PreRender" AutoGenerateColumns="false"  ShowFooter="True" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="作業">
                                        <ItemTemplate>
                                            <asp:Button ID="btn_change" CssClass="btn-success btnw2" OnClick="btn_change_Click" Text="異動" CommandName="按鈕屬性"  runat="server" />
                                            <asp:Button ID="btnDelMain" CssClass="btn-danger btnw2" Text="刪除" OnClick="btnDelMain_Click" CommandName="按鈕屬性" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="款源" DataField="OVC_ISOURCE" />
                                    <asp:BoundField HeaderText="預算小計(系統自動計算)" DataField="ONB_MONEY" />
                                    <asp:BoundField HeaderText="預算是否核定" DataField="OVC_ISAPPR" />
                                </Columns>
                                <FooterStyle HorizontalAlign="Left" />
                            </asp:GridView>
                            
                            <table class="table table-bordered text-left">
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="款源"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoOVC_IKIND" RepeatDirection="Horizontal" RepeatLayout="Flow"  runat="server"> 
                                            <asp:ListItem Value="1">國防預算</asp:ListItem>
                                            <asp:ListItem Value="2">非國防預算</asp:ListItem>
                                        </asp:RadioButtonList>
                                        
                                        <asp:TextBox ID="txtOVC_POI_IBDG" CssClass="tb tb-l" runat="server" ></asp:TextBox>
                                        <asp:HyperLink ID="lnkExample" runat="server">範例</asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="金額"></asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Text="幣別："></asp:Label>
                                        <asp:DropDownList ID="drpOVC_CURRENT" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                        <asp:Label CssClass="control-label" runat="server" Text="匯率："></asp:Label>
                                        <asp:TextBox ID="txtONB_RATE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server" Text="預算小計("></asp:Label>
                                        <asp:Label CssClass="control-label text-red" runat="server" Text="系統自動計算，外幣換新台幣採四捨五入至「元」"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server" Text=")"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="預算是否核定"></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="drpApproved" CssClass="tb tb-m position-left" OnSelectedIndexChanged="drpApproved_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                            <asp:ListItem Value="1">預算已核定</asp:ListItem>
                                            <asp:ListItem Value="2">預算未核定</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label CssClass="control-label position-left" runat="server" Text="奉准日期："></asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_PUR_DAPPR_PLAN" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label position-left" runat="server" Text="奉准文號："></asp:Label>
                                        <asp:TextBox ID="txtOVC_PUR_APPR_PLAN" CssClass="tb tb-l position-left" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnSave" CssClass="btn-success btnw4" OnClick="btnSave_Click" runat="server" Text="新增存檔" />
                                <asp:Button ID="btnModify" CssClass="btn-success btnw4" OnClick="btnModify_Click" Visible="false" runat="server" Text="異動存檔" />
                                <asp:Button ID="btnDetails" CssClass="btn-success" runat="server" OnClick="btnDetails_Click" Text="預算年度分配明細表" />
                                <asp:Button ID="btnReturn" CssClass="btn-success" runat="server" OnClick="btnReturn_Click" Text="回物資申請書編制作業" /><br><br>
                                <asp:Label CssClass="control-label text-red" runat="server" Text="預算分配年度月份、金額"></asp:Label>
                            </div>
                            <asp:GridView ID="GV_BudgetAllocation" CssClass="table table-striped border-top text-center" OnPreRender="GV_BudgetAllocation_PreRender" AutoGenerateColumns="false" runat="server" ShowFooter="True">
                                <Columns>
                                    <asp:TemplateField HeaderText="作業">
                                        <ItemTemplate>
                                            <asp:Button ID="btn_changeDetail" CssClass="btn-success btnw2" Text="異動" OnClick="btn_changeDetail_Click" CommandName="按鈕屬性" runat="server" />
                                            <asp:Button ID="btnDelDetail" CssClass="btn-danger btnw2" Text="刪除" OnClick="btnDelDetail_Click" CommandName="按鈕屬性" runat="server" />
                                            <asp:HiddenField ID="hidKind" runat="server" Value='<%#Eval("OVC_IKIND")%>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="款源" DataField="OVC_ISOURCE" />
                                    <asp:BoundField HeaderText="預算科目" DataField="OVC_IBDGPJNAME" />
                                    <asp:BoundField HeaderText="年度" DataField="OVC_YY" />
                                    <asp:BoundField HeaderText="月份" DataField="OVC_MM" />
                                    <asp:BoundField HeaderText="預劃金額" DataField="ONB_MBUD" />
                                </Columns>
                            </asp:GridView>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td><asp:Button ID="btnNew" CssClass="btn-success btnw2" OnClick="btnNew_Click" runat="server" Text="新增" /></td>
                                    <td><asp:DropDownList ID="drpOVC_ISOURCE" CssClass="tb tb-s" OnSelectedIndexChanged="drpOVC_ISOURCE_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Text="代號："></asp:Label>
                                        <asp:DropDownList ID="drpOVC_POI_IBDG_2" CssClass="tb tb-m" OnSelectedIndexChanged="drpOVC_POI_IBDG2_SelectedIndexChanged" AutoPostBack="True" runat="server"></asp:DropDownList><br> <%--預算科目(代號)(代碼L8國防預算及代碼L9非國防預算,當為非國防預算時預算科目代號=預算科目名稱)--%>
                                        <asp:Label CssClass="control-label" runat="server" Text="名稱："></asp:Label>
                                        <asp:TextBox ID="txtOVC_PJNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td><asp:DropDownList ID="drpOVC_YY" CssClass="tb" runat="server"></asp:DropDownList></td>
                                    <td><asp:DropDownList ID="drpOVC_MM" CssClass="tb" runat="server">
                                        <asp:ListItem>01</asp:ListItem>
                                            <asp:ListItem>02</asp:ListItem>
                                            <asp:ListItem>03</asp:ListItem>
                                            <asp:ListItem>04</asp:ListItem>
                                            <asp:ListItem>05</asp:ListItem>
                                            <asp:ListItem>06</asp:ListItem>
                                            <asp:ListItem>07</asp:ListItem>
                                            <asp:ListItem>08</asp:ListItem>
                                            <asp:ListItem>09</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>11</asp:ListItem>
                                            <asp:ListItem>12</asp:ListItem>
                                        </asp:DropDownList></td>
                                    <td><asp:TextBox ID="txtONB_MBUD" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                </tr>
                            </table>
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
