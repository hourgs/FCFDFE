<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_A1A_2.aspx.cs" Inherits="FCFDFE.pages.MTS.A.MTS_A1A_2" %>
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
                    <div>進口軍品運輸交接單-修改</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Panel ID="PnUpdate" runat="server"></asp:Panel>
                            <div class="text-right" style="padding: 10px;">
                                <asp:LinkButton CssClass="btn-success btnw6" OnClick="btnBack_Click" Text="回交接單管理" runat="server"></asp:LinkButton>
                            </div>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">交接單編號</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left" style="width:85%">
                                        <asp:Label ID="lblOVC_IHO_NO" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">清運方法</asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:DropDownList ID="drpOVC_TRANS_TYPE" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Label CssClass="control-label" runat="server">起運地點</asp:Label>
                                    </td>
                                    <td class="text-left" style="width: 35%">
                                        <asp:TextBox ID="txtOVC_START_PLACE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width:15%">
                                        <asp:Label CssClass="control-label" runat="server">運達地點</asp:Label>
                                    </td>
                                    <td class="text-left" style="width: 35%">
                                        <asp:TextBox ID="txtOVC_ARRIVE_PLACE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">起運時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtODT_START_DATE" CssClass="tb tb-date" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">抵運時間</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:Label ID="lblODT_ARRIVE_DATE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">接收單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:HiddenField ID="txtOVC_RECEIVE_DEPT_CDE" runat=Server/>  
                                        <asp:Label ID="lblOVC_RECEIVE_DEPT_CDE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">接轉地區</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:HiddenField ID="txtOVC_TRANSER_DEPT_CDE" runat=Server/>  
                                        <asp:Label ID="lblOVC_TRANSER_DEPT_CDE" CssClass="control-label" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">總件數</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtONB_TOTAL_QUANITY" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">計量單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_QUANITY_UNIT" CssClass="tb tb-s" OnSelectedIndexChanged="drpOVC_QUANITY_UNIT_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        <asp:TextBox ID="txtOVC_QUANITY_UNIT" CssClass="tb tb-xs" Visible="false" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">總體積</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtONB_TOTAL_VOLUME" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">計量單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_VOLUME_UNIT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">總重量</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtONB_TOTAL_WEIGHT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">計量單位</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:DropDownList ID="drpOVC_WEIGHT_UNIT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">船機名稱</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_SHIP_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">船機航次</asp:Label>
                                    </td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_VOYAGE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">點收情形</asp:Label>
                                    </td>
                                    <td colspan="3" class="td-inner-table">
                                        <table class="table table-bordered text-center table-inner">
                                            <tr>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">超出</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">短少</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">破損</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="control-label" runat="server">實收</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtONB_OVERFLOW" CssClass="tb tb-full" runat="server">0</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtONB_LESS" CssClass="tb tb-full" runat="server">0</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtONB_BROKEN" CssClass="tb tb-full" runat="server">0</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtONB_ACTUAL_RECEIVE" CssClass="tb tb-full" runat="server">0</asp:TextBox>
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
                                    <td colspan="4" class="td-inner-table text-left">
                                        <asp:GridView ID="GVTBGMT_IRD_DETAIL_1" DataKeyNames="OVC_IRDDETAIL_SN" CssClass="table table-striped border-top table-inner text-center" AutoGenerateColumns="false"
                                            OnPreRender="GVTBGMT_IRD_DETAIL_1_PreRender" OnRowCommand="GVTBGMT_IRD_DETAIL_1_RowCommand" OnRowDataBound="GVTBGMT_IRD_DETAIL_RowDataBound" runat="server">
                                            <Columns>
                                                <asp:TemplateField HeaderText="項次" ItemStyle-CssClass="text-center" >
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex + 1%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:BoundField HeaderText="提單號碼" DataField="OVC_BLD_NO" ReadOnly="true"/>--%>
                                                <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                                    <ItemTemplate>
                                                        <!--BLD顯示新方法-->
                                                        <asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="購案號">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOVC_PURCH_NO" Text='<%#( Eval("OVC_PURCH_NO").ToString() )%>' runat="server" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtOVC_PURCH_NO" Text='<%#( Eval("OVC_PURCH_NO").ToString() )%>' CssClass="tb tb-m" runat="server" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="品名" DataField="OVC_CHI_NAME" ReadOnly="true"/>
                                                <asp:BoundField HeaderText="箱號" DataField="OVC_BOX_NO" ReadOnly="true"/>
                                                <asp:BoundField HeaderText="實收件數" DataField="ONB_ACTUAL_RECEIVE" ReadOnly="true"/>
                                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                                    <ItemTemplate>
                                                        <asp:Button CssClass="btn-success btnw2" Text="編輯" CommandName="btnModify" UseSubmitBehavior="False" runat="server"/>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Button CssClass="btn-success btnw2" Text="更新" CommandName="btnUpdate" UseSubmitBehavior="False" runat="server"/>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-center">
                                                    <ItemTemplate>
                                                        <asp:Button CssClass="btn-danger btnw2" Text="移除" CommandName="btnDel" UseSubmitBehavior="False" OnClientClick="if (confirm('確定移除此資料?') == false) return false;" runat="server"/>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Button CssClass="btn-success btnw2" Text="取消" CommandName="btnCancel" UseSubmitBehavior="False" runat="server"/>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:Button ID="btnOther" CssClass="btn-success btnw8" style="margin: 10px 20px;" Text="顯示新增明細選單" OnClick="btnOther_Click" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Button CssClass="btn-warning" Text="更新軍品運輸交接單" OnClick="btnEdit_IHO_Click" runat="server"/>
                                    </td>
                                </tr>
                            </table>
                            <%--<div class="text-center">
                            </div>--%>
                            <asp:Panel ID="PnTable" runat="server">
                                <table class="table table-bordered text-center">
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">提單編號：</asp:Label> 
                                            <asp:TextBox ID="txtOVC_BLD_NO" CssClass="tb tb-s" OnTextChanged="txtOVC_BLD_NO_TextChanged" AutoPostBack="true" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button CssClass="btn-success btnw6" Text="查詢提單箱號" OnClick="btnQuery_Click" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Button CssClass="btn-warning btnw9" Text="加入軍品運輸交換單" OnClick="btnNew_Detail_Click" runat="server"/>
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="GVTBGMT_IRD_DETAIL_2" DataKeyNames="OVC_IRDDETAIL_SN" CssClass="table data-table table-striped border-top text-center" AutoGenerateColumns="false" OnPreRender="GVTBGMT_IRD_DETAIL_2_PreRender" OnRowDataBound="GVTBGMT_IRD_DETAIL_RowDataBound" runat="server">
                                    <Columns>
                                        <asp:TemplateField HeaderText="選取" ItemStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" CssClass="radioButton rb-complex" Text="" CommandName="" CommandArgument='' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField HeaderText="提單號碼" DataField="OVC_BLD_NO" />--%>
                                        <asp:TemplateField HeaderText="提單編號" ItemStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <!--BLD顯示新方法-->
                                                <asp:HyperLink ID="hlkOVC_BLD_NO" Text='<%# Eval("OVC_BLD_NO")%>' runat="server"></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="購案號" DataField="OVC_PURCH_NO" />
                                        <asp:BoundField HeaderText="品名" DataField="OVC_CHI_NAME" />
                                        <asp:BoundField HeaderText="分運單位" DataField="OVC_DEPT_CDE" />
                                        <asp:BoundField HeaderText="箱號" DataField="OVC_BOX_NO" />
                                        <asp:BoundField HeaderText="實收件數" DataField="ONB_ACTUAL_RECEIVE" />
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
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
