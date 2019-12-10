<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_B22_1.aspx.cs" Inherits="FCFDFE.pages.MTS.B.MTS_B22_1" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <script type="text/javascript">
        function CallPrint(strid)
        {
         var prtContent = document.getElementById(strid);
         var strOldOne=prtContent.innerHTML;
         var WinPrint = window.open('','','letf=0,top=0,width=1,height=1,toolbar=0,scrollbars=0,status=0');
         WinPrint.document.write(prtContent.innerHTML);
         WinPrint.document.close();
         WinPrint.focus();
         WinPrint.print();
         WinPrint.close();
         prtContent.innerHTML=strOldOne;
        }
    </script>
    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    投保通知書管理
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">投保通知書編號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_EINN_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">外運資料表編號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_EDF_NO" CssClass="tb tb-m text-toUpper" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">投保日期</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:RadioButtonList ID="rdoODT_INS_DATE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1" Selected="True">不限定</asp:ListItem>
                                            <asp:ListItem Value="2" Text=""></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_INS_DATE_S" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label" runat="server">&nbsp;至&nbsp;&nbsp;</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_INS_DATE_E" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">建檔日期</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:RadioButtonList ID="rdoODT_CREATE_DATE" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="1" Selected="True">不限定</asp:ListItem>
                                            <asp:ListItem Value="2" Text=""></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_CREATE_DATE_S" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                        <asp:Label CssClass="control-label" runat="server">&nbsp;至&nbsp;&nbsp;</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_CREATE_DATE_E" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">軍種</asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="drpOVC_MILITARY_TYPE" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button cssclass="btn-success btnw2" OnClick="btnQuery_Click" Text="查詢" runat="server" />
                                <asp:Button ID="btnPrint2" OnClick="btnPrint2_Click" cssclass="btn-success btnw6" Text="列印查詢結果" Visible="False" runat="server" /><br /><br />
                            </div>
                            <asp:GridView ID="GV_TBGMT_EINN" DataKeyNames="EINN_SN" CssClass="table data-table table-striped border-top" AutoGenerateColumns="false" 
                                OnPreRender="GV_TBGMT_EINN_PreRender" OnRowCommand="GV_TBGMT_EINN_RowCommand" OnRowDataBound="GV_TBGMT_EINN_RowDataBound" OnRowCreated="GV_TBGMT_EINN_RowCreated" runat="server">
                                <Columns>
                                     <%--<asp:BoundField HeaderText="項次" DataField="" />--%>
                                     <asp:TemplateField HeaderText="項次" ItemStyle-CssClass="text-center" >
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1%>
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:BoundField HeaderText="投保通知書編號" DataField="OVC_EINN_NO" />
                                     <%--<asp:BoundField HeaderText="外運資料表編號" DataField="OVC_EDF_NO" />--%>
                                    <asp:TemplateField HeaderText="外運資料表編號" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <!--EDF顯示新方法-->
                                            <asp:HiddenField ID="lblEDF_SN" Value='<%# Eval("EDF_SN")%>' runat="server" />
                                            <asp:HyperLink ID="hlkOVC_EDF_NO" Text='<%# Eval("OVC_EDF_NO")%>' runat="server"></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:BoundField HeaderText="案號或採購文號" DataField="OVC_PURCH_NO" />
                                     <asp:BoundField HeaderText="物資價值" DataField="ONB_ITEM_VALUE" />
                                     <asp:BoundField HeaderText="出口日期" DataField="ODT_CREATE_DATE" />
                                     <asp:BoundField HeaderText="投保金額" DataField="ONB_INS_AMOUNT" />
                                     <asp:BoundField HeaderText="保費(台幣)" DataField="OVC_FINAL_INS_AMOUNT" />
                                     <asp:BoundField HeaderText="運輸工具" DataField="" />
                                     <asp:BoundField HeaderText="啟運港口" DataField="OVC_START_PORT" />
                                     <asp:BoundField HeaderText="目的港口" DataField="OVC_ARRIVE_PORT" />
                                     <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center" >
                                         <ItemTemplate>
                                             <asp:Button CssClass="btn-warning" Text="修改" CommandName="dataModify" runat="server"/>
                                             <asp:Button CssClass="btn-danger" Text="刪除" CommandName="dataDel" runat="server"/>
                                             <asp:Button CssClass="btn-success" Text="列印" CommandName="dataPrint" runat="server"/>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <%--<asp:TemplateField HeaderText="" >
                                         <ItemTemplate>
                                             <asp:Button CssClass="btn-warning" Text="修改" CommandName="dataModify" runat="server"/>
                                         </ItemTemplate>
                                     </asp:TemplateField> 
                                     <asp:TemplateField HeaderText="" >
                                         <ItemTemplate>
                                             <asp:Button CssClass="btn-danger" Text="刪除" CommandName="dataDel" runat="server"/>
                                         </ItemTemplate>
                                     </asp:TemplateField> 
                                         <asp:TemplateField HeaderText="" >
                                         <ItemTemplate>
                                             <asp:Button CssClass="btn-success" Text="列印" CommandName="dataPrint" runat="server"/>
                                         </ItemTemplate>
                                     </asp:TemplateField> --%>
                                 </Columns>
                             </asp:GridView>
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
