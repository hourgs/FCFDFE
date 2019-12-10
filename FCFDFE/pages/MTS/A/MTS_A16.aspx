<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A16.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A16" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
        function CallPrint(strid) {
            var prtContent = document.getElementById(strid);
            var strOldOne = prtContent.innerHTML;
            var WinPrint = window.open();
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
            prtContent.innerHTML = strOldOne;
        }
    </script>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <div>運況管制日報表管理</div>
                </header>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">接收日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_IDR_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">接轉地區</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_TRANSER_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button CssClass="btn-success btnw2" OnClick="btnQuery_Click" Text="查詢" runat="server" />
                            </div>
                            <br />
                            <div>
                            <asp:GridView ID="GVTBGMT_ICR" CssClass="table data-table table-striped border-top dt-fixed" DataKeyNames="OVC_BLD_NO" AutoGenerateColumns="false" OnPreRender="GVTBGMT_ICR_PreRender" OnRowDataBound="GVTBGMT_ICR_RowDataBound" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="項次" ItemStyle-CssClass="text-center" >
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>
                                            <%--<a href="javascript: OpenWindow_BLDDATA('<%# FCommon.getEncryption(Eval("OVC_BLD_NO").ToString()) %>');">
                                                <%# Eval("OVC_BLD_NO")%>
                                            </a>--%>
                                            <%--<a href="javascript:window.open('BLDDATA.aspx?OVC_BLD_NO=<%# Eval("OVC_BLD_NO")%>', null, 'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=540,height=520,left=200,top=80');">
                                                <%# Eval("OVC_BLD_NO")%></a>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="案號" DataField="OVC_PURCH_NO" />
                                    <asp:BoundField HeaderText="品名" DataField="OVC_CHI_NAME" />
                                    <asp:BoundField HeaderText="進口日期" DataField="ODT_IMPORT_DATE" />
                                    <asp:BoundField HeaderText="報關日期" DataField="ODT_CUSTOM_DATE" />
                                    <asp:BoundField HeaderText="通關日期" DataField="ODT_PASS_CUSTOM_DATE" />
                                    <asp:BoundField HeaderText="拆櫃日期" DataField="ODT_UNPACKING_DATE" />
                                    <asp:BoundField HeaderText="清運日期" DataField="ODT_TRANSFER_DATE" />
                                    <asp:BoundField HeaderText="接收日期" DataField="ODT_RECEIVE_DATE" />
                                    <asp:BoundField HeaderText="清運方式" DataField="OVC_TRANS_TYPE" />
                                    <asp:BoundField HeaderText="接收單位" DataField="OVC_RECEIVE_DEPT_CODE" />
                                    <asp:BoundField HeaderText="件數" DataField="ONB_QUANITY" />
                                    <asp:BoundField HeaderText="重量噸" DataField="ONB_WEIGHT"  DataFormatString="{0:#,###.00}" />
                                    <asp:BoundField HeaderText="體積噸" DataField="ONB_VOLUME"  DataFormatString="{0:#,##0.00}" />
                                    <asp:BoundField HeaderText="備考" DataField="OVC_NOTE" />
                                </Columns>
                            </asp:GridView>
                                </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                    <asp:Button ID="btnPrint" CssClass="btn-success btnw2" Text="列印" OnClientClick="CallPrint('divPrint');" OnClick="btnPrint_Click" runat="server" Visible="false" />
                </footer>
            </section>
        </div>
    </div>
</asp:Content>
