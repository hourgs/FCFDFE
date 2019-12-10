<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_A13.aspx.cs" Inherits="FCFDFE.pages.CIMS.A.CIMS_A13" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <div class="row">
        <div style="width: 800px; margin: auto;">
            <asp:Panel ID="Panel1" runat="server"></asp:Panel>
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <div>代理商查詢功能</div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <asp:Panel ID="condition" runat="server">
                    <div class="panel-body" style="border: solid 2px;">
                        <div class="form" style="border: 5px;">
                            <div class="cmxform form-horizontal tasi-form">
                                <!--網頁內容-->
                                <asp:Label CssClass="subtitle" runat="server">請選填搜尋條件後，按下[查詢]即可</asp:Label>
                                <table class="table table-bordered text-left">
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">申請序號/編號</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtREGNO_query" CssClass="tb tb-l" runat="server">
                                            </asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">國外廠商名稱</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtVEN_NAME_query" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">代理商名稱</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtVEN_NAME_T_query" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">代理項目(產品)</asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtAGENT_ITEM_query" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server">國外授權在台效期</asp:Label></td>
                                        <td>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtAUTH_DATE_S_query" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                <span class="add-on"><i class="icon-calendar"></i></span>
                                                <asp:Label CssClass="control-label position-left" runat="server">&emsp;至&emsp;</asp:Label>
                                            </div>
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtAUTH_DATE_E_query" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                <span class="add-on"><i class="icon-calendar"></i></span>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>

                    <footer class="panel-footer" style="text-align: center;">
                        <!--網頁尾-->
                        <asp:Button ID="btnQuery" CssClass="btn-success btnw2" runat="server" Text="查詢" OnClick="btnQuery_Click" />
                        &emsp;
                    <asp:Button ID="btnReset" CssClass="btn-default btnw2" runat="server" Text="取消" />
                    </footer>
                </asp:Panel>
            </section>
            <asp:Panel ID="querytable" runat="server" Visible="false">
                <section class="panel">
                    <header class="title">
                        查詢結果
                    </header>
                    <div class="panel-body" style="border: solid 2px;">
                        <div class="form" style="border: 5px;">
                            <div class="cmxform form-horizontal tasi-form">
                                <asp:GridView ID="GV_VENAGENT" DataKeyNames="REGNO" CssClass="table data-table table-striped border-top table-bordered" OnPreRender="GV_VENAGENT_PreRender" OnRowCommand="GV_VENAGENT_RowCommand" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:BoundField HeaderText="申請序號/編號" DataField="REGNO" ItemStyle-Width="10%" />
                                        <asp:BoundField HeaderText="國外廠商名稱" DataField="VEN_NAME" ItemStyle-Width="15%" />
                                        <asp:BoundField HeaderText="代理商名稱" DataField="VEN_NAME_T" ItemStyle-Width="15%" />
                                        <asp:BoundField HeaderText="代理項目" DataField="AGENT_ITEM" ItemStyle-Width="40%" />
                                        <asp:BoundField HeaderText="在台效期" DataField="AbleTime" ItemStyle-Width="15%" />
                                        <asp:TemplateField HeaderText="詳細資料">
                                            <ItemTemplate>
                                                <asp:Button CssClass="btn-info btnw2" CommandName="Detail" runat="server" Text="查看" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </section>
            </asp:Panel>
            <asp:Panel ID="DataDetail" runat="server" Visible="false">
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <asp:Label CssClass="subtitle" runat="server">國外廠商資料及代號</asp:Label>
                            <table class="table table-bordered text-left">
                                <tr>
                                    <td style="width: 30%">
                                        <asp:Label CssClass="control-label" runat="server">申請序號/編號</asp:Label></td>
                                    <td class="text-left" style="width: 70%">
                                        <asp:TextBox ID="txtREGNO" CssClass="tb tb-m" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%">
                                        <asp:Label CssClass="control-label" runat="server">公司名稱</asp:Label></td>
                                    <td class="text-left" style="width: 70%">
                                        <asp:TextBox ID="txtVEN_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">部門</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_DEPT" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">代號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_CODE" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">地址</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_ADDR" CssClass="tb tb-full" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">電話</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_TEL" CssClass="tb tb-m" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">電傳</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_FAX" CssClass="tb tb-m" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">負責人</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_BOSS" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                            </table>

                            <asp:Label CssClass="subtitle" runat="server">在台代理/代表商資料及代號</asp:Label>
                            <table class="table table-bordered text-left">
                                <tr>
                                    <td style="width: 30%">
                                        <asp:Label CssClass="control-label" runat="server">在台中文名稱</asp:Label></td>
                                    <td class="text-left" style="width: 70%">
                                        <asp:TextBox ID="txtVEN_NAME_T" CssClass="tb tb-m" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">在台英文名稱</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_ENAME_T" CssClass="tb tb-m" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">代號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_CODE_T" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">營業地址</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_ADDR_T" CssClass="tb tb-full" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">電話</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_TEL_T" CssClass="tb tb-m" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">電傳</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_FAX_T" CssClass="tb tb-m" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">負責人</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_BOSS_T" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                            </table>
                            <br>
                            <table class="table table-bordered text-left">
                                <tr>
                                    <td style="width: 30%">
                                        <asp:Label CssClass="control-label" runat="server">國外廠商授權在台辦理/代表效期</asp:Label></td>
                                    <td class="text-left" style="width: 70%">
                                        <%--<div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">--%>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtAUTH_DATE_S" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                            <asp:Label CssClass="control-label position-left" runat="server">&emsp;至&emsp;</asp:Label>
                                        </div>
                                        <%--<div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">--%>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtAUTH_DATE_E" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">授權範圍(權限)</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtAUTH_RANGE" CssClass="tb tb-m" runat="server" Text="在台代理推介產品">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">代理項品(產品)</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtAGENT_ITEM" CssClass="tb tb-l" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">核定在台代理/代表效期</asp:Label></td>
                                    <td class="text-left">
                                        <%--<div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years" style="float:left">--%>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtAPPR_DATE_S" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                            <asp:Label CssClass="control-label position-left" runat="server">&emsp;至&emsp;</asp:Label>
                                        </div>
                                        <%--<div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years" style="float:left">--%>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtAPPR_DATE_E" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">附註事項</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_MEMO" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server" Text="本文件僅供於授權有效期限及授權範圍內，從事產品推介業務，不得作為投標、議價、簽約之依據。"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="panel-footer" style="text-align: center;">
                                <!--網頁尾-->
                    <asp:Button ID="btnReport" CssClass="btn-default btnw6" runat="server" Text="產生報表" Onclick="btnReport_Click"/>
                                &emsp;
                    <asp:Button ID="btnCancel" CssClass="btn-default btnw2" runat="server" Text="取消" Onclick="btnCancel_Click"/>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
