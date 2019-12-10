<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A18_2.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A18_2" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <div>進口物資管制接配紀錄表-修改</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">提單號碼</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_BLD_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">應收件數/單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <%--<asp:TextBox ID="txtONB_QUANITY" CssClass="tb tb-m" OnKeyPress="txtKeyNumber()" runat="server"></asp:TextBox>--%>
                                        <asp:Label ID="lblONB_QUANITY" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_QUANITY_UNIT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">船(機)名</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_SHIP_NAME" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">重量/單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <%--<asp:TextBox ID="txtONB_WEIGHT" CssClass="tb tb-m" OnKeyPress="txtKeyNumber()" runat="server"></asp:TextBox>--%>
                                        <asp:Label ID="lblONB_WEIGHT" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_WEIGHT_UNIT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">航次</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_VOYAGE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">體積/單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <%--<asp:TextBox ID="txtONB_VOLUME" CssClass="tb tb-m" OnKeyPress="txtKeyNumber()" runat="server"></asp:TextBox>--%>
                                        <asp:Label ID="lblONB_VOLUME" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_VOLUME_UNIT" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">購案號</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:Label ID="lblOVC_PURCH_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: middle;">
                                        <asp:Label CssClass="control-label" runat="server">貨櫃號碼</asp:Label>
                                        <asp:Button ID="btnCreate_CTN" CssClass="btn-success btnw4" OnClick="btnCreate_CTN_Click" Text="新增貨櫃" runat="server" />
                                    </td>
                                    <td colspan="3" class="td-inner-table">
                                        <asp:Panel ID="PnMessage_CTN" CssClass="text-left" runat="server"></asp:Panel>
                                        <asp:GridView ID="GVTBGMT_CTN" DataKeyNames="OVC_CONTAINER_NO" CssClass="table table-striped border-top table-inner" AutoGenerateColumns="false" OnRowCommand="GVTBGMT_CTN_RowCommand" OnRowDataBound="GVTBGMT_CTN_RowDataBound" OnPreRender="GVTBGMT_CTN_PreRender" runat="server">
                                            <Columns>
                                                <%--<asp:BoundField HeaderText="貨櫃號碼" DataField="OVC_CONTAINER_NO"></asp:BoundField>--%>
                                                <%--<asp:BoundField HeaderText="尺寸" DataField="ONB_SIZE"/>--%>
                                                <asp:TemplateField HeaderText="貨櫃號碼">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOVC_CONTAINER_NO" Text='<%#( Eval("OVC_CONTAINER_NO").ToString() )%>' runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <%--<asp:Label ID="lblOVC_CONTAINER_NO" Text='<%#( Eval("OVC_CONTAINER_NO").ToString() )%>' runat="server" />--%>
                                                        <asp:TextBox ID="txtOVC_CONTAINER_NO" Text='<%#( Eval("OVC_CONTAINER_NO").ToString() )%>' CssClass="tb tb-m" runat="server" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="尺寸">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%#( Eval("ONB_SIZE").ToString() )%>' runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblONB_SIZE" Text='<%#( Eval("ONB_SIZE").ToString() )%>' Visible="false" runat="server" />
                                                        <asp:DropDownList ID="drpONB_SIZE" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                                        <%--<asp:TextBox Text='<%#( Eval("ONB_SIZE").ToString() )%>' CssClass="tb tb-s" runat="server" />--%>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnModify" CssClass="btn-success btnw2" Text="編輯" CommandName="DataEdit" runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Button ID="btnSave" CssClass="btn-success btnw2" Text="儲存" CommandName="DataSave" runat="server" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnDel" CssClass="btn-danger btnw2" Text="刪除" CommandName="DataDelete" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Button ID="btnCancel" CssClass="btn-success btnw2" Text="取消" CommandName="DataCancel" runat="server" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">進口日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_ARRIVE_PORT_DATE" CssClass="control-label" runat="server"></asp:Label>
                                        <%--<div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_ARRIVE_PORT_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>--%>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">提單/拆櫃日期</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_CLEAR_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">清驗情形</asp:Label>
                                    </td>
                                    <td colspan="3" class="td-inner-table">
                                        <table class="table table-bordered text-center table-inner">
                                            <tr>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">實收</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">溢卸</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">短少</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">破損</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtONB_ACTUAL_RECEIVE" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtONB_OVERFLOW" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtONB_LESS" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtONB_BROKEN" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:TextBox ID="txtOVC_NOTE" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">分運明細</asp:Label><br>
                                        <asp:Button ID="btnCreate_DETAIL" CssClass="btn-success btnw4" Text="箱號輸入" OnClick="btnCreate_DETAIL_Click" runat="server" />
                                        <%--<asp:CheckBox ID="chkOVC_BOX_NO" CssClass="radioButton" runat="server" />未繫結<br>--%>
                                        <%--<asp:Button ID="btnOVC_BOX_NO_Sub" cssclass="btn-success btnw4" runat="server" Text="箱號代入" />--%>
                                    </td>
                                    <td colspan="3" class="td-inner-table">
                                        <asp:Panel ID="PnMessage_DETAIL" CssClass="text-left" runat="server"></asp:Panel>
                                        <asp:GridView ID="GVTBGMT_DETAIL" DataKeyNames="OVC_IRDDETAIL_SN" CssClass="table table-striped border-top table-inner" AutoGenerateColumns="false" OnRowCommand="GVTBGMT_DETAIL_RowCommand" OnRowDataBound="GVTBGMT_DETAIL_RowDataBound" OnPreRender="GVTBGMT_DETAIL_PreRender" runat="server">
                                            <Columns>
                                                <%--<asp:BoundField HeaderText="分運單位" DataField="OVC_DEPT_CDE" />--%>
                                                <asp:TemplateField HeaderText="分運單位">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%#( Eval("OVC_ONNAME").ToString() )%>' runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtOVC_DEPT_CDE" CssClass="tb tb-s" Text='<%#( Eval("OVC_DEPT_CDE").ToString() )%>' hidden="true" runat="server"></asp:TextBox>
                                                        <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-s" Text='<%#( Eval("OVC_ONNAME").ToString() )%>' runat="server"></asp:TextBox>
                                                        <asp:Button ID="btnQueryOVC_REQ_DEPT_CDE" CssClass="btn-success btnw2" OnClientClick="OpenWindow()" Text="單位" runat="server" /><!--OnClientClick 於後端設定-->
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:BoundField HeaderText="箱號" DataField="OVC_BOX_NO" />--%>
                                                <asp:TemplateField HeaderText="箱號">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%#( Eval("OVC_BOX_NO").ToString() )%>' runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtOVC_BOX_NO" Text='<%#( Eval("OVC_BOX_NO").ToString() )%>' CssClass="tb tb-s" runat="server" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:BoundField HeaderText="實收" DataField="ONB_ACTUAL_RECEIVE" />--%>
                                                <asp:TemplateField HeaderText="實收">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%#( Eval("ONB_ACTUAL_RECEIVE").ToString() )%>' runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtONB_ACTUAL_RECEIVE" Text='<%#( Eval("ONB_ACTUAL_RECEIVE").ToString() )%>' CssClass="tb tb-xs" OnKeyPress="txtKeyNumber()" runat="server" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:BoundField HeaderText="溢卸" DataField="ONB_OVERFLOW" />--%>
                                                <asp:TemplateField HeaderText="溢卸">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%#( Eval("ONB_OVERFLOW").ToString() )%>' runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtONB_OVERFLOW" Text='<%#( Eval("ONB_OVERFLOW").ToString() )%>' CssClass="tb tb-xs" OnKeyPress="txtKeyNumber()" runat="server" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:BoundField HeaderText="短少" DataField="ONB_LESS" />--%>
                                                <asp:TemplateField HeaderText="短少">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%#( Eval("ONB_LESS").ToString() )%>' runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtONB_LESS" Text='<%#( Eval("ONB_LESS").ToString() )%>' CssClass="tb tb-xs" OnKeyPress="txtKeyNumber()" runat="server" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:BoundField HeaderText="破損" DataField="ONB_BROKEN" />--%>
                                                <asp:TemplateField HeaderText="破損">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%#( Eval("ONB_BROKEN").ToString() )%>' runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtONB_BROKEN" Text='<%#( Eval("ONB_BROKEN").ToString() )%>' CssClass="tb tb-xs" OnKeyPress="txtKeyNumber()" runat="server" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:Button CssClass="btn-success btnw2" Text="編輯" CommandName="DataEdit" runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Button CssClass="btn-success btnw2" Text="儲存" CommandName="DataSave" runat="server" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:Button CssClass="btn-danger btnw2" Text="刪除" CommandName="DataDelete" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Button CssClass="btn-success btnw2" Text="取消" CommandName="DataCancel" runat="server" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button ID="btnModify" CssClass="btn-warning" Text="更新接配紀錄表" OnClick="btnModify_Click" runat="server" />
                                <asp:Button ID="btnHome" CssClass="btn-warning" Text="回首頁" OnClick="btnHome_Click" runat="server" />
                            </div>
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
