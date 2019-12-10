<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_H17.aspx.cs" Inherits="FCFDFE.pages.MTS.H.MTS_H17" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        .lbl-gridview {
            display: block;
        }
    </style>
    <div class="row">
        <div style="width: 1150px; margin:auto;">
            <section class="panel">
                <header class="title">
                    <%--<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>--%>
                    <asp:Label ID="lblOVC_SECTION" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblDEPT" runat="server"></asp:Label>
                    <asp:DropDownList  ID="drpOVC_SECTION" CssClass="tb tb-s" runat="server">
                            <%--<asp:ListItem>基隆地區</asp:ListItem>--%>
                            <%--<asp:ListItem>桃園地區</asp:ListItem>--%>
                            <%--<asp:ListItem>高雄分遣組</asp:ListItem>--%>
                    </asp:DropDownList>
                    海空運進出口運費管制表 
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">查詢日期</asp:Label>
                                    </td>
                                    <td colspan="7" >
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtQueryDate1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class="add-on"><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label" runat="server">&nbsp;&nbsp;至&nbsp;&nbsp;</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtQueryDate2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class="add-on"><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">資料期程</asp:Label>
                                    </td>
                                    <td colspan="7" >
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOdtCreateDate1" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class="add-on"><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label" runat="server">&nbsp;&nbsp;至&nbsp;&nbsp;</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOdtCreateDate2" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class="add-on"><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">承運航商</asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList  ID="drpOVC_SHIP_COMPANY" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">航次</asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtOVC_VOYAGE" CssClass="tb tb-m" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <%--<asp:Label CssClass="control-label" style="margin-left: 20px;" runat="server">報表序號：</asp:Label>--%>
                                        <%--<asp:TextBox ID="txtIdrSn" CssClass="tb tb-s" runat="server"></asp:TextBox>--%>
                                    </td>
                                </tr> 
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnQuery" cssclass="btn-success btnw2" OnClick="btnQuery_Click" Text="查詢" runat="server" />
                                <%--<asp:Button ID="btnQuery_narmal" cssclass="btn-success btnw2" OnClick="btnQuery_narmal_Click" runat="server" Text="查詢" />--%> 
                                <asp:Button ID="btnPrintCenter" cssclass="btn-success" OnClick="btnPrint_Click" CommandArgument="OVC_NOTE_CENTER" Text="列印中心報表" Visible="false" runat="server" />
                                <asp:Button ID="btnPrintCompany" cssclass="btn-success" OnClick="btnPrint_Click" CommandArgument="OVC_NOTE_COMPANY" Text="列印航商報表" Visible="false" runat="server" /><br /><br />
                            </div>
                        <%--</div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                </footer>
            </section>
            <section class="panel">
                <asp:Panel ID="Panel1" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">--%>
                            <asp:Panel ID="pnEdit" runat="server">
                                <table class="table table-bordered" style="margin-top: 20px;">
                                    <tr>
                                        <td class="text-center">
                                            <asp:Label CssClass="control-label" runat="server">帳單收穫日期</asp:Label>
                                        </td>
                                        <td colspan="3" >
                                            <asp:CheckBox ID="chkODT_ACQUIRE_DATE" CssClass="radioButton" Checked="true" runat="server"/>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtODT_ACQUIRE_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                                <span class="add-on"><i class="icon-calendar"></i></span>
                                            </div>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label CssClass="control-label" runat="server">簽證退商日期</asp:Label>
                                        </td>
                                        <td colspan="3" >
                                            <asp:CheckBox ID="chkODT_RETURN_DATE" CssClass="radioButton" Checked="true" runat="server"/>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtODT_RETURN_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                                <span class="add-on"><i class="icon-calendar"></i></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="text-center">
                                            <asp:Label CssClass="control-label" runat="server">中心備考</asp:Label>
                                        </td>
                                        <td colspan="3" >
                                            <asp:CheckBox ID="chkOVC_NOTE_CENTER" CssClass="radioButton" Checked="true" runat="server"/>
                                            <asp:TextBox ID="txtOVC_NOTE_CENTER" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="text-center">
                                            <asp:Label CssClass="control-label" runat="server">航商備考</asp:Label>
                                        </td>
                                        <td colspan="3" >
                                            <asp:CheckBox ID="chkOVC_NOTE_COMPANY" CssClass="radioButton" Checked="true" runat="server"/>
                                            <asp:TextBox ID="txtOVC_NOTE_COMPANY" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="text-center">
                                            <asp:Label CssClass="control-label" runat="server">報表顯示</asp:Label>
                                        </td>
                                        <td colspan="7" >
                                            <asp:CheckBox ID="chkONB_SHOW" CssClass="radioButton" Checked="true" runat="server"/>
                                            <asp:DropDownList  ID="drpONB_SHOW" CssClass="tb tb-s" runat="server">
                                                <asp:ListItem Value="1">是</asp:ListItem>
                                                <asp:ListItem Value="0">否</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <div class="text-center">
                                    <asp:Button ID="btnUpdate" cssclass="btn-warning" runat="server" OnClick="btnUpdate_Click" Text="更新勾選提單資料列" /><br /><br />
                                </div>
                            </asp:Panel>
                            <asp:GridView ID="GV_TBGMT_SCC" DataKeyNames="OVC_BLD_NO" CssClass="table table-bordered table-striped border-top text-center" style="margin-top: 20px;" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_SCC_PreRender" OnRowDataBound="GV_TBGMT_SCC_RowDataBound" OnRowCommand="GV_TBGMT_SCC_RowCommand" runat="server">
                                <Columns>
                                    <%--<asp:BoundField DataField="column1" HeaderText="航商<br/>提單號碼" HtmlEncode="False"/>--%>
                                    <asp:TemplateField HeaderText="航商<br/>提單編號" ItemStyle-CssClass="text-left">
                                        <ItemTemplate>
                                            <asp:Label CssClass="lbl-gridview" Text='<%#( Eval("OVC_SHIP_COMPANY").ToString() )%>' runat="server" />
                                            <asp:Label CssClass="lbl-gridview" Text='<%#( Eval("OVC_BLD_NO").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="column2" HeaderText="船名<br/>航次" HtmlEncode="False"/>--%>
                                    <asp:TemplateField HeaderText="船名<br/>航次" ItemStyle-CssClass="text-left">
                                        <ItemTemplate>
                                            <asp:Label CssClass="lbl-gridview" Text='<%#( Eval("OVC_SHIP_NAME").ToString() )%>' runat="server" />
                                            <asp:Label CssClass="lbl-gridview" Text='<%#( Eval("OVC_VOYAGE").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="column3" HeaderText="裝貨港<br/>最後港口" HtmlEncode="False"/>--%>
                                    <asp:TemplateField HeaderText="裝貨港<br/>最後港口">
                                        <ItemTemplate>
                                            <asp:Label CssClass="lbl-gridview" Text='<%#( Eval("OVC_START_PORT").ToString() )%>' runat="server" />
                                            <asp:Label CssClass="lbl-gridview" Text='<%#( Eval("OVC_LAST_START_PORT").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="column4" HeaderText="離港日<br/>結關前一日" HtmlEncode="False"/>--%>
                                    <asp:TemplateField HeaderText="離港日<br/>結關前一日">
                                        <ItemTemplate>
                                            <asp:Label CssClass="lbl-gridview" Text='<%#( Eval("ODT_START_DATE_Text").ToString() )%>' runat="server" />
                                            <asp:Label CssClass="lbl-gridview" Text='<%#( Eval("ODT_LAST_START_DATE_Text").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="column5" HeaderText="USD<br/>TWD" HtmlEncode="False"/>--%>
                                    <asp:TemplateField HeaderText="USD<br/>TWD" ItemStyle-CssClass="text-right">
                                        <ItemTemplate>
                                            <asp:Label CssClass="lbl-gridview" Text='<%#( Eval("USD_CARRIAGE_Text").ToString() )%>' runat="server" />
                                            <asp:Label CssClass="lbl-gridview" Text='<%#( Eval("TWD_CARRIAGE_Text").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="ODT_ACQUIRE_DATE" HeaderText="帳單收穫日期"/>--%>
                                    <asp:TemplateField HeaderText="帳單收穫日期">
                                        <ItemTemplate>
                                            <asp:Label CssClass="lbl-gridview" Text='<%#( Eval("ODT_ACQUIRE_DATE_Text").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtODT_ACQUIRE_DATE" CssClass="tb tb-date" Text='<%#( Eval("ODT_ACQUIRE_DATE_Text").ToString() )%>' runat="server"></asp:TextBox>
                                                <span class="add-on"><i class="icon-calendar"></i></span>
                                            </div>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="ODT_RETURN_DATE" HeaderText="簽證退商日期"/>--%>
                                    <asp:TemplateField HeaderText="簽證退商日期">
                                        <ItemTemplate>
                                            <asp:Label CssClass="lbl-gridview" Text='<%#( Eval("ODT_RETURN_DATE_Text").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtODT_RETURN_DATE" CssClass="tb tb-date" Text='<%#( Eval("ODT_RETURN_DATE_Text").ToString() )%>' runat="server"></asp:TextBox>
                                                <span class="add-on"><i class="icon-calendar"></i></span>
                                            </div>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="中心支付日期" DataField="ODT_PAID_DATE_Text" ReadOnly="true"/>
                                    <%--<asp:BoundField DataField="column9" HeaderText="中心備考<br/>船商備考" HtmlEncode="False"/>--%>
                                    <asp:TemplateField HeaderText="中心備考<br/>船商備考">
                                        <ItemTemplate>
                                            <asp:Label CssClass="lbl-gridview" Text='<%#( Eval("OVC_NOTE_CENTER").ToString() )%>' runat="server" />
                                            <asp:Label CssClass="lbl-gridview" Text='<%#( Eval("OVC_NOTE_COMPANY").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtOVC_NOTE_CENTER" CssClass="tb tb-xs" Text='<%#( Eval("OVC_NOTE_CENTER").ToString() )%>' runat="server" />
                                            <asp:TextBox ID="txtOVC_NOTE_COMPANY" CssClass="tb tb-xs" style="margin-top: 5px;" Text='<%#( Eval("OVC_NOTE_COMPANY").ToString() )%>' runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="ONB_SHOW" HeaderText="報表顯示" />--%>
                                    <asp:TemplateField HeaderText="報表顯示">
                                        <ItemTemplate>
                                            <asp:Label CssClass="lbl-gridview" Text='<%#( Eval("ONB_SHOW_Text").ToString() )%>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:HiddenField ID="lblONB_SHOW" Value='<%#( Eval("ONB_SHOW").ToString() )%>' runat="server" />
                                            <asp:CheckBox ID="chkONB_SHOW" CssClass="radioButton" Text="是" runat="server"/>
                                            <%--<asp:DropDownList  ID="drpONB_SHOW" CssClass="tb tb-xs" runat="server">
                                                <asp:ListItem Value="1">是</asp:ListItem>
                                                <asp:ListItem Value="0">否</asp:ListItem>
                                            </asp:DropDownList>--%>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" CssClass="btn-success" Text="編輯" CommandName="btnEdit" runat="server"/>
                                            <asp:Button ID="btnUpdate" CssClass="btn-success" Text="更新" CommandName="btnUpdate" Visible="false" runat="server"/>
                                            <asp:Button ID="btnCancel" CssClass="btn-success" Text="取消" CommandName="btnCancel" Visible="false" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="" ItemStyle-Width="4em" >
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn-success" Text="編輯" CommandName="DataEdit" runat="server"/>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button CssClass="btn-success" Text="更新" CommandName="DataSave" runat="server"/>
                                            <asp:Button CssClass="btn-success" Text="取消" style="margin-top: 5px;" CommandName="DataCancel" runat="server"/>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" >
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" CssClass="radioButton" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <%--<asp:GridView ID="GV_TBGMT_SCC_normal" CssClass="table data-table table-striped border-top data-table" AutoGenerateColumns="false" OnPreRender="GV_TBGMT_SCC_normal_PreRender" OnRowDataBound="GV_TBGMT_SCC_RowDataBound" runat="server">
                                 <Columns>
                                    <asp:BoundField DataField="column1" HeaderText="航商<br/>提單號碼" HtmlEncode="False"/>
                                    <asp:BoundField DataField="column2" HeaderText="船名<br/>航次" HtmlEncode="False"/>
                                    <asp:BoundField DataField="column3" HeaderText="裝貨港<br/>最後港口" HtmlEncode="False"/>
                                    <asp:BoundField DataField="column4" HeaderText="離港日<br/>結關前一日" HtmlEncode="False" DataFormatString = "{0:   MM/dd}"/>
                                    <asp:BoundField DataField="column5" HeaderText="USD<br/>TWD" HtmlEncode="False"/>
                                    <asp:BoundField DataField="column6" HeaderText="帳單收穫日期" DataFormatString = "{0:yy/MM/dd}"/>
                                    <asp:BoundField DataField="column7" HeaderText="簽證退商日期" DataFormatString = "{0:yy/MM/dd}"/>
                                    <asp:BoundField DataField="column8" HeaderText="中心支付日期" DataFormatString = "{0:yy/MM/dd}"/>
                                    <asp:BoundField DataField="column9" HeaderText="中心備考<br/>船商備考" HtmlEncode="False"/>
                                    <asp:BoundField DataField="column10" HeaderText="報表顯示" />
                                </Columns>
                            </asp:GridView>--%>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
