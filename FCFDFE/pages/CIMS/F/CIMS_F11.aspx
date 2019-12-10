<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_F11.aspx.cs" Inherits="FCFDFE.pages.CIMS.F.CIMS_F11" %>

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
                    <div>廠商資料查詢</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <asp:Panel ID="search" runat="server">
                        <div class="form" style="border: 5px;">
                            <div class="cmxform form-horizontal tasi-form">
                                <!--網頁內容-->
                                <div class="subtitle">請選填欲搜尋的條件，按下[搜尋]即可搜尋</div>
                                <table class="table table-bordered text-left">
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">顯示所有暫編廠商</asp:Label></td>
                                        <td>
                                            <asp:Button ID="btnQuery_temporary" CssClass="btn-success btnw2" runat="server" Text="查詢" OnClick="btnQuery_temporary_Click" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">廠商名稱(關鍵字)</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtOVC_VEN_TITLE_query" CssClass="tb tb-m" runat="server">
                                            </asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">廠商簡稱(關鍵字)</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtOVC_NVEN_query" CssClass="tb tb-m" runat="server">
                                            </asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">廠商代碼(統編)</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtOVC_VEN_CST_query" CssClass="tb tb-m" runat="server">
                                            </asp:TextBox></td>
                                    </tr>
                                </table>
                                <div align="center">
                                    <asp:Button ID="btnAdd" CssClass="btn-success btnw4" runat="server" Text="新增廠商" OnClick="btnAdd_Click"  />
                                    &emsp;
                                    <asp:Button ID="btnQuery" CssClass="btn-success btnw2" runat="server" Text="查詢" OnClick="btnQuery_Click" />
                                    &emsp;
                    <asp:Button ID="btnReset" CssClass="btn-default btnw2" runat="server" Text="清除" OnClick="btnReset_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="form" style="border: 5px;">
                            <asp:GridView ID="GV_TBM1203" DataKeyNames="OVC_VEN_CST" CssClass="table data-table table-striped border-top table-bordered" AutoGenerateColumns="false" runat="server" OnPreRender="GV_TBM1203_PreRender" OnRowCommand="GV_TBM1203_RowCommand" Visible="false">
                                <Columns>
                                    <asp:BoundField HeaderText="項次" DataField="RANK" ItemStyle-Width="5%" />
                                    <asp:TemplateField HeaderText="廠商代碼(統編)" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" Text='<%# Eval("OVC_VEN_CST")%>' CommandName="OVC_VEN_CST" runat="server">LinkButton</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="廠商簡稱" DataField="OVC_NVEN" ItemStyle-Width="15%" />
                                    <asp:BoundField HeaderText="廠商名稱" DataField="OVC_VEN_TITLE" ItemStyle-Width="20%" />
                                    <asp:BoundField HeaderText="聯絡人" DataField="PERFORM_NAME" ItemStyle-Width="8%" />
                                    <asp:BoundField HeaderText="電話" DataField="OVC_VEN_ITEL" ItemStyle-Width="12%" />
                                    <asp:BoundField HeaderText="住址" DataField="OVC_VEN_ADDRESS" ItemStyle-Width="20%" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="Detail" runat="server" Visible="false">
                        <div id="editcontrol" runat="server">
                        <div class="subtitle">暫編編號改正式統一編號</div>
                            </div>
                        <table id="tbEIN_change" class="table table-bordered control-label" style="text-align: center" runat="server">
                            <tr>
                                <td width="12%" bgcolor="#FF99FF">
                                    <div align="left">暫編編號</div>
                                </td>
                                <td width="24%" align="left">
                                    <asp:Label ID="EIN_before" runat="server"></asp:Label></td>
                                <td width="11%" bgcolor="#FF99FF">
                                    <div align="left">正式統一編號</div>
                                </td>
                                <td width="53%" align="left">
                                    <asp:TextBox ID="EIN_after" runat="server" MaxLength="8" CssClass="tb tb-m"></asp:TextBox>
                                    <asp:Button ID="EIN_Change" CssClass="btn-success btnw4" runat="server" Text="編號轉換" OnClick="EIN_Change_Click" />
                                </td>
                            </tr>
                        </table>
                        <div id="editcontorl1" runat="server">
                        <div class="subtitle">廠商資訊維護</div>
                            </div>
                        <div id="addcontrol1" runat="server">
                            <div class="subtitle">廠商資訊新增</div>
                            </div>
                        <table id="tbEdit" class="table table-bordered control-label" style="text-align: center">
                            <tr>
                                <td width="12%" bgcolor="#FF99FF">
                                    <div align="left">統一編號</div>
                                </td>
                                <td width="24%" align="left">
                                    <asp:Label ID="txtOVC_VEN_CST" runat="server"></asp:Label>
                                    <asp:TextBox ID="addOVC_VEN_CST" runat="server" CssClass="tb tb-m" Visible="false" MaxLength="8"></asp:TextBox>
                                </td>
                                <td width="11%" bgcolor="#FF99FF">
                                    <div align="left">廠商名稱</div>
                                </td>
                                <td width="53%" align="left" colspan="3">
                                    <asp:TextBox ID="txtOVC_VEN_TITLE" runat="server" CssClass="tb tb-full"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td width="12%" bgcolor="#FF99FF">
                                    <div align="left">地址一</div>
                                </td>
                                <td width="54%" align="left" colspan="3">
                                    <asp:TextBox ID="txtOVC_VEN_ADDRESS" runat="server" CssClass="tb tb-full"></asp:TextBox></td>
                                <td width="13%" bgcolor="#FF99FF">
                                    <div align="left">廠商簡稱</div>
                                </td>
                                <td width="21%" align="left">
                                    <asp:TextBox ID="txtOVC_NVEN" runat="server" CssClass="tb tb-m"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td width="12%" bgcolor="#FF99FF">
                                    <div align="left">地址二</div>
                                </td>
                                <td width="54%" align="left" colspan="3">
                                    <asp:TextBox ID="txtOVC_VEN_ADDRESS_1" runat="server" CssClass="tb tb-full"></asp:TextBox></td>
                                <td width="13%" bgcolor="#FF99FF">
                                    <div align="left">建檔日</div>
                                </td>
                                <td width="21%" align="left">
                                    <asp:Label ID="txtOVC_PUR_CREATE" runat="server"></asp:Label>
                            </tr>
                            <tr>
                                <td width="12%" bgcolor="#FF99FF">
                                    <div align="left">電話號碼</div>
                                </td>
                                <td width="24%" align="left">
                                    <asp:TextBox ID="txtOVC_VEN_ITEL" runat="server" CssClass="tb tb-m"></asp:TextBox></td>
                                <td width="11%" bgcolor="#FF99FF">
                                    <div align="left">傳真</div>
                                </td>
                                <td width="19%" align="left">
                                    <asp:TextBox ID="txtOVC_FAX_NO" runat="server" CssClass="tb tb-m"></asp:TextBox></td>
                                <td width="13%" bgcolor="#FF99FF">
                                    <div align="left">負責人/聯絡人</div>
                                </td>
                                <td width="21%" align="left">
                                    <asp:TextBox ID="txtOVC_BOSS" runat="server" CssClass="tb tb-m"></asp:TextBox><br />
                                    <asp:TextBox ID="txtPERFORM_NAME" runat="server" CssClass="tb tb-m"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td width="12%" bgcolor="#FF99FF">
                                    <div align="left">經濟部編號</div>
                                </td>
                                <td width="24%" align="left">
                                    <asp:TextBox ID="txtGINGE_VEN_CST" runat="server" CssClass="tb tb-m"></asp:TextBox>
                                </td>
                                <td width="11%" bgcolor="#FF99FF">
                                    <div align="left">國管代號</div>
                                </td>
                                <td width="19%" align="left">
                                    <asp:TextBox ID="txtCAGE" runat="server" CssClass="tb tb-m"></asp:TextBox>
                                </td>
                                <td width="13%" bgcolor="#FF99FF">
                                    <div align="left">國管回饋日</div>
                                </td>
                                <td width="21%" align="left">
                                    <asp:TextBox ID="txtCAGE_DATE" runat="server" CssClass="tb tb-m"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td width="12%" bgcolor="#FF99FF">
                                    <div align="left">主要營業項目</div>
                                </td>
                                <td colspan="5" align="left">
                                    <asp:TextBox ID="txtOVC_MAIN_PRODUCT" runat="server" CssClass="tb tb-full"></asp:TextBox></td>
                            </tr>
                        </table>
                        <div align="center">
                            <asp:Button ID="Addok" CssClass="btn-default btnw2" runat="server" Text="新增" OnClick="Addok_Click" />
                            &emsp;
                            <asp:Button ID="Editok" CssClass="btn-default btnw4" runat="server" Text="修正存檔" OnClick="Editok_Click" />
                            &emsp;
                            <asp:Button ID="Return" CssClass="btn-default btnw2" runat="server" Text="返回" OnClick="Return_Click" />
                        </div>
                    </asp:Panel>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->

                </footer>
            </section>
        </div>
    </div>
</asp:Content>
