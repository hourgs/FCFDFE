<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_G13.aspx.cs" Inherits="FCFDFE.pages.MTS.G.MTS_G13" %>
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
                    <!--標題-->
                    <div>國外物資接轉外購軍品運雜費統計表</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">    
                                <tr>
                                    <td style="width:15%"><asp:Label CssClass="control-label" runat="server">結報申請表編號</asp:Label></td>
                                    <td class="text-left" style="width:35%">
                                        <asp:TextBox ID="txtOVC_TOF_NO" CssClass="tb tb-m" OnTextChanged="txtOVC_TOF_NO_TextChanged" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width:15%"><asp:Label CssClass="control-label" runat="server">軍種別</asp:Label></td>
                                    <td class="text-left" style="width:35%">
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-s" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">進出口別</asp:Label></td>
                                    <td class="text-left" >
                                        <asp:DropDownList ID="drpOVC_IMP_OR_EXP" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Selected="True">不限定</asp:ListItem>
                                            <asp:ListItem>進口</asp:ListItem>
                                            <asp:ListItem>出口</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td ><asp:Label CssClass="control-label" runat="server">運保費別</asp:Label></td>
                                    <td class="text-left" >
                                        <asp:DropDownList ID="drpONB_CARRIAGE" CssClass="tb tb-s" AutoPostBack="true" runat="server">
                                            <asp:ListItem>不限定</asp:ListItem>
                                            <asp:ListItem value="海運">海運費</asp:ListItem>
                                            <asp:ListItem Value="空運">空運費</asp:ListItem>
                                            <asp:ListItem Value="保險費">保險費</asp:ListItem>
                                            <asp:ListItem Value="作業費">作業費</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="drpOVC_FINAL_INS_AMOUNT" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>不限定</asp:ListItem>
                                            <asp:ListItem>長榮海運</asp:ListItem>
                                            <asp:ListItem>陽明海運</asp:ListItem>
                                            <asp:ListItem>非合約航商</asp:ListItem>
                                        </asp:DropDownList>
                                         <asp:DropDownList ID="drpOVC_FINAL_INS_AMOUNT2" CssClass="tb tb-s" runat="server">
                                             <asp:ListItem>不限定</asp:ListItem>
                                             <asp:ListItem>中華航空</asp:ListItem>
                                             <asp:ListItem>長榮航空</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">預算科目</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_BUDGET" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Selected="True">不限定</asp:ListItem>
                                            <asp:ListItem>維持門010105</asp:ListItem>
                                            <asp:ListItem>投資門150110</asp:ListItem>
                                            <asp:ListItem>維持門010106</asp:ListItem>
                                            <asp:ListItem>投資門150111</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">付款區分</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_PAYMENT_TYPE" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Selected="True">不限定</asp:ListItem>
                                            <asp:ListItem>已付款</asp:ListItem>
                                            <asp:ListItem>未付款</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">結報申請表日期</asp:Label></td>
                                    <td class="text-left" colspan="3">
                                        <asp:RadioButtonList ID="rdoODT_APPLY_DATE" CssClass="radioButton position-left" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem>當日作業</asp:ListItem>
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" >
											<asp:TextBox ID="txtODT_APPLY_DATE_S" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
											<div class="add-on"><i class="icon-calendar"></i></div>
										</div>
                                        <asp:Label CssClass="control-label position-left" runat="server">&emsp;至&emsp;</asp:Label>
                                        <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" >
											<asp:TextBox ID="txtODT_APPLY_DATE_E" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
											<div class="add-on"><i class="icon-calendar"></i></div>
										</div> 
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center">
								<asp:Button ID="btnQuery" cssclass="btn-success btnw2" OnClick="btnQuery_Click" runat="server" Text="查詢" />&emsp;
                                <asp:Button ID="btnPrint" cssclass="btn-success btnw2" OnClick="btnPrint_Click" Visible="false" runat="server" Text="列印" />
							</div>
                        </div>
                    </div>
                    <br>
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <div style="text-align:center">
                                <asp:GridView ID="GVTBGMT_TOF" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GVTBGMT_TOF_PreRender" runat="server">
								    <Columns>
									    <asp:BoundField HeaderText="結報申請表編號" DataField="OVC_INF_NO" />
									    <asp:BoundField HeaderText="軍種" DataField="OVC_CLASS_NAME" />
									    <asp:BoundField HeaderText="進出口別" DataField="OVC_IE_TYPE" />
									    <asp:BoundField HeaderText="運保費別" DataField="Insurance_Premium" />
									    <asp:BoundField HeaderText="預算科目" DataField="OVC_BUDGET" />
									    <asp:BoundField HeaderText="金額(新台幣)" DataField="ONB_AMOUNT"  DataFormatString = "{0:#,##0.000}" />
									    <asp:BoundField HeaderText="付款區分" DataField="OVC_IS_PAID" />
									    <asp:BoundField HeaderText="備考" DataField="OVC_SHIP_COMPANY" />
									    <asp:BoundField HeaderText="結報日期" DataField="ODT_APPLY_DATE" DataFormatString = "{0:yyyy/MM/dd}" />
								    </Columns>
							    </asp:GridView>
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
