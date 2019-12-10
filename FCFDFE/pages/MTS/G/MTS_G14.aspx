<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_G14.aspx.cs" Inherits="FCFDFE.pages.MTS.G.MTS_G14" %>
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
                    <div>進口軍售購案已投保未投保統計表</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">    
                                <tr>
                                    <td style="width:25%;"><asp:Label CssClass="control-label" runat="server">啟運日期</asp:Label></td>
                                    <td class="text-left">
                                        <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" >
											<asp:TextBox ID="txtODT_START_DATE_S" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
											<div class="add-on"><i class="icon-calendar"></i></div>
										</div>
                                        <asp:Label CssClass="control-label position-left" runat="server">&emsp;至&emsp;</asp:Label>
                                        <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" >
											<asp:TextBox ID="txtODT_START_DATE_E" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
											<div class="add-on"><i class="icon-calendar"></i></div>
										</div> 
                                    </td>
                                </tr>
                                <tr> 
                                    <td><asp:Label CssClass="control-label" runat="server">軍種別</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-s" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td ><asp:Label CssClass="control-label" runat="server">案件種類</asp:Label></td>
                                    <td class="text-left" >
                                        <asp:DropDownList ID="drpOVC_PURCHASE_TYPE" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Selected="True">不限定</asp:ListItem>
                                            <asp:ListItem>軍售</asp:ListItem>
                                            <asp:ListItem>商購</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td ><asp:Label CssClass="control-label" runat="server">投保與否</asp:Label></td>
                                    <td class="text-left" >
                                        <asp:DropDownList ID="drpOVC_IS_PAY" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Selected="True" Value="1">已投保</asp:ListItem>
                                            <asp:ListItem Value="0">未投保</asp:ListItem>
                                        </asp:DropDownList>          
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center">
								<asp:Button ID="btnQuery" OnClick="btnQuery_Click" cssclass="btn-success btnw2" runat="server" Text="查詢" />&emsp;
                                <asp:Button ID="btnPrint2" OnClick="btnPrint2_Click" cssclass="btn-success btnw2" runat="server" Visible="false" Text="列印" />
                                <asp:Button ID="btnPrint" OnClick="btnPrint_Click" cssclass="btn-success btnw2" Visible="false" runat="server" Text="列印" />
							</div>
                        </div>
                    </div>
                    <br>
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <div style="text-align:center">	
                                <asp:GridView ID="GVTBGMT_IINN" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GVTBGMT_IINN_PreRender" runat="server">
								    <Columns>

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

