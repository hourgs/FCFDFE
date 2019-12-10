<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_G11.aspx.cs" Inherits="FCFDFE.pages.MTS.G.MTS_G11" %>
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
                    <div>國防部國防採購室-運案查詢</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">進出口別</asp:Label></td>
                                    <td class="text-left" colspan="3">
                                        <asp:RadioButtonList ID="rdoOVC_IMP_OR_EXP" CssClass="radioButton" RepeatDirection="Horizontal" AutoPostBack="true"  RepeatLayout="Flow" GroupName="xx" runat="server" >
                                            <asp:ListItem Value="Import" Selected="True">進口</asp:ListItem>
                                            <asp:ListItem Value="Export" >出口</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label id="lbdate" CssClass="control-label" Text="進口日期" runat="server" ></asp:Label></td>
                                    <td class="text-left" colspan="3">
                                        <asp:RadioButtonList ID="rdoODT_IMPORT_DATE" CssClass="radioButton position-left" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem>當日作業</asp:ListItem>
                                            <asp:ListItem Selected="True"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_IMPORT_DATE_S" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>

                                        <asp:Label CssClass="control-label position-left" runat="server">&emsp;至&emsp;</asp:Label>
                                         
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_IMPORT_DATE_E" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">軍種別</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-m" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">海空運別</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_SEA_OR_AIR" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>不限定</asp:ListItem>
                                            <asp:ListItem>海運</asp:ListItem>
                                            <asp:ListItem>空運</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">承運航商</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_SHIP_COMPANY" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>不限定</asp:ListItem>
                                            <asp:ListItem>中華航空</asp:ListItem>
                                            <asp:ListItem>長榮航空</asp:ListItem>
                                            <asp:ListItem>長榮海運</asp:ListItem>
                                            <asp:ListItem>陽明海運</asp:ListItem>
                                            <asp:ListItem>非合約航商</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">接轉單位</asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_TRANSER_DEPT_CDE" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>不限定</asp:ListItem>
                                            <asp:ListItem>基隆地區</asp:ListItem>
                                            <asp:ListItem>桃園地區</asp:ListItem>
                                            <asp:ListItem>高雄分遣組</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                 <tr id="export_row" runat="server">
                                     <td><asp:Label CssClass="control-label" runat="server">外運資料表編號</asp:Label></td>
                                     <td colspan="3" class="text-left">
                                          <asp:TextBox ID="txtOVC_EDF_NO" OnTextChanged="txtOvcEdfNo_TextChanged"  CssClass="tb tb-l" runat="server"></asp:TextBox>
                                     </td>
                                 </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">提單編號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_BLD_NO" OnTextChanged="txtOvcBldNo_TextChanged"  CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                    <td><asp:Label CssClass="control-label" runat="server">案號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_PURCH_NO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr> 
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="機敏單位"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="DropSmartUint" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="1">是</asp:ListItem>
                                            <asp:ListItem Value="0"  Selected="True">否</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center">
								<asp:Button ID="btnQuery" cssclass="btn-success btnw2" runat="server" OnClick="btnQuery_Click" Text="查詢" />&emsp;
                                <asp:Button ID="btnPrint" cssclass="btn-success btnw2" runat="server" Visible="false" OnClick="btnPrint_Click" Text="列印" />
                                <asp:Button ID="btnPrintExport" cssclass="btn-success btnw2" runat="server" Visible="false" OnClick="btnPrintExport_Click" Text="列印" />
							</div>
                        </div>
                    </div>
                    <br>
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <div style="text-align:center">
                            <asp:GridView ID="GV_TBGMT_BLD" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_BLD_PreRender"   runat="server">
								    <Columns>
									    <asp:BoundField HeaderText="軍種" DataField="OVC_CLASS_NAME" />
									    <asp:BoundField HeaderText="海空運" DataField="OVC_SEA_OR_AIR" />
									    <asp:BoundField HeaderText="承運航商" DataField="OVC_SHIP_COMPANY" />
                                        <asp:TemplateField HeaderText="提單案號"  >
                                            <ItemTemplate>
                                                            <a ID="link" href="javascript:var win=window.open('BLDDATA.aspx?OVC_BLD_NO=<%# Eval("OVC_BLD_NO")%>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=700,left=0,top=0');" >
                                                            <%# Eval("OVC_BLD_NO")%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
									    <%--<asp:BoundField HeaderText="提單編號" DataField="OVC_BLD_NO"   />--%>
									    <asp:BoundField HeaderText="案號" DataField="OVC_PURCH_NO" />
									    <asp:BoundField HeaderText="啟運港埠" DataField="OVC_START_PORT" />
									    <asp:BoundField HeaderText="抵運港埠" DataField="OVC_ARRIVE_PORT" />
									    <asp:BoundField HeaderText="報關日期" DataField="ODT_CUSTOM_DATE" DataFormatString = "{0:yyyy/MM/dd}"/>
									    <asp:BoundField HeaderText="進口日期" DataField="ODT_IMPORT_DATE" DataFormatString = "{0:yyyy/MM/dd}" />
									    <asp:BoundField HeaderText="通關日期" DataField="ODT_PASS_CUSTOM_DATE" DataFormatString = "{0:yyyy/MM/dd}"/>
                                        <asp:BoundField HeaderText="清運日期" DataField="ODT_TRANSFER_DATE" DataFormatString = "{0:yyyy/MM/dd}"/>
                                        <asp:BoundField HeaderText="接收日期" DataField="ODT_RECEIVE_DATE" DataFormatString = "{0:yyyy/MM/dd}"/>
								    </Columns>
							    </asp:GridView>
                                <asp:GridView ID="GV_TBGMT_BLD_export" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_BLD2_PreRender"    runat="server">
								    <Columns>
									    <asp:BoundField HeaderText="軍種" DataField="OVC_CLASS_NAME" />
									    <asp:BoundField HeaderText="海空運" DataField="OVC_SEA_OR_AIR" />
									    <asp:BoundField HeaderText="承運航商" DataField="OVC_SHIP_COMPANY" />
                                        <asp:BoundField HeaderText="外運資料表編號" DataField="OVC_EDF_NO" />
                                        <asp:TemplateField HeaderText="提單案號"  >
                                            <ItemTemplate>
                                                            <a ID="link" href="javascript:var win=window.open('BLDDATA.aspx?OVC_BLD_NO=<%# Eval("OVC_BLD_NO")%>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=600,height=700,left=0,top=0');" >
                                                            <%# Eval("OVC_BLD_NO")%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
									    <%--<asp:BoundField HeaderText="提單編號" DataField="OVC_BLD_NO"   />--%>
									    <asp:BoundField HeaderText="案號" DataField="OVC_PURCH_NO" />
									    <asp:BoundField HeaderText="啟運港埠" DataField="OVC_START_PORT" />
									    <asp:BoundField HeaderText="抵運港埠" DataField="OVC_ARRIVE_PORT" />
									    <asp:BoundField HeaderText="委運單位函文日期" DataField="ODT_REQUIRE_DATE" DataFormatString = "{0:yyyy/MM/dd}"/>
									    <asp:BoundField HeaderText="中文函文免稅日期" DataField="ODT_PROCESS_DATE" DataFormatString = "{0:yyyy/MM/dd}" />
									    <asp:BoundField HeaderText="進艙期間" DataField="ODT_STORED_DATE" DataFormatString = "{0:yyyy/MM/dd}"/>
                                        <asp:BoundField HeaderText="報關日期" DataField="ODT_EXP_DATE" DataFormatString = "{0:yyyy/MM/dd}"/>
                                        <asp:BoundField HeaderText="起運日期" DataField="ODT_START_DATE" DataFormatString = "{0:yyyy/MM/dd}"/>
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