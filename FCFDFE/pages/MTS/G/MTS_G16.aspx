<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_G16.aspx.cs" Inherits="FCFDFE.pages.MTS.G.MTS_G16" %>
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
                    <div>年度海運運費查詢</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">    
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">提單編號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width:20%;"><asp:Label CssClass="control-label" runat="server">航商</asp:Label></td>
                                    <td class="text-left" style="width:30%;">
                                        <asp:DropDownList ID="drpOVC_SHIP_COMPANY" CssClass="tb tb-m   " runat="server">
                                        </asp:DropDownList>
                                    </td>  
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">軍種</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-s" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:20%;"><asp:Label CssClass="control-label" runat="server">品名分類</asp:Label></td>
                                    <td class="text-left" style="width:30%;">
                                        <asp:DropDownList ID="drpOVC_ITEM_TYPE" CssClass="tb tb-m   " runat="server">
                                        </asp:DropDownList>
                                    </td>  
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">最後離境日期</asp:Label></td>
                                    <td class="text-left" colspan="3">
                                        <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" >
											<asp:TextBox ID="txtODT_LAST_START_DATE_S" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
											<div class="add-on"><i class="icon-calendar"></i></div>
										</div>
                                        <asp:Label CssClass="control-label position-left" runat="server">&emsp;至&emsp;</asp:Label>
                                        <div class="input-append date position-left datepicker" data-date="<%=DateTime.Now%>" >
											<asp:TextBox ID="txtODT_LAST_START_DATE_E" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
											<div class="add-on"><i class="icon-calendar"></i></div>
										</div> 
                                        <asp:CheckBox ID="chkODT_ACT_ARRIVE_DATE" Text="不限定日期" CssClass="radioButton position-left" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center">
								<asp:Button ID="btnQuery" OnClick="btnQuery_Click" cssclass="btn-success btnw2" runat="server" Text="查詢" />&emsp;
                                <asp:Button ID="btnPrint" OnClick="btnPrint_Click" cssclass="btn-success btnw2" Visible="false" runat="server" Text="列印" />
							</div>
                        </div>
                    </div>
                    <br>
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <div style="text-align:center">
                                <asp:GridView ID="GVTBGMT_BLD" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GVTBGMT_BLD_PreRender" runat="server">
								    <Columns>
									    <asp:BoundField HeaderText="軍種" DataField="OVC_CLASS_NAME" />
									    <asp:BoundField HeaderText="品名分類" DataField="OVC_CHI_NAME" />
									    <asp:BoundField HeaderText="提單編號" DataField="OVC_BLD_NO" />
                                        <asp:BoundField HeaderText="最後離境/抵運港口" DataField="OVC_PORT_CHI_NAME" />
									    <asp:BoundField HeaderText="最後離境日期" DataField="ODT_ACT_ARRIVE_DATE"  DataFormatString = "{0:yyyy/MM/dd}"/>
                                        <asp:BoundField HeaderText="運費" DataField="ONB_CARRIAGE" DataFormatString = "{0:#,##0.00}" />
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
