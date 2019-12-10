<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_A11.aspx.cs" Inherits="FCFDFE.pages.CIMS.A.CIMS_A11" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <div class="row">
        <div style="width: 800px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->
                    <asp:Label CssClass="control-label" runat="server">代理商新增功能</asp:Label>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->                    
                            <asp:Label CssClass="subtitle" runat="server">國外廠商資料及代號</asp:Label>
                            <table class="table table-bordered text-left">
                                <tr>
                                    <td style="width:30%"><asp:Label CssClass="control-label" runat="server">申請序號/編號</asp:Label></td>
                                    <td class="text-left" style="width:70%">
                                        <asp:TextBox ID="txtREGNO" CssClass="tb tb-m" runat="server" ReadOnly="true"></asp:TextBox>
                                        <asp:CheckBox ID="Manual" runat="server" Autopostback="true"/>
                                        <asp:Label runat="server" Text="手動輸入"></asp:Label>
                                        <asp:TextBox ID="txtREGNO_Manual" CssClass="tb tb-m" runat="server" visible="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:30%"><asp:Label CssClass="control-label" runat="server">公司名稱</asp:Label></td>
                                    <td class="text-left" style="width:70%">
                                        <asp:TextBox ID="txtVEN_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">部門</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_DEPT" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">代號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_CODE" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">地址</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_ADDR" CssClass="tb tb-full" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">電話</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_TEL" CssClass="tb tb-m" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">電傳</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_FAX" CssClass="tb tb-m" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">負責人</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_BOSS" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                            </table>

                            <asp:Label CssClass="subtitle" runat="server">在台代理/代表商資料及代號</asp:Label>
                            <table class="table table-bordered text-left" >
                                <tr>
                                    <td style="width:30%"><asp:Label CssClass="control-label" runat="server">公司中文名稱</asp:Label></td>
                                    <td class="text-left" style="width:70%">
                                        <asp:TextBox ID="txtVEN_NAME_T" CssClass="tb tb-m" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">公司英文名稱</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_ENAME_T" CssClass="tb tb-m" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">代號</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_CODE_T" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">營業地址</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_ADDR_T" CssClass="tb tb-full" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">電話</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_TEL_T" CssClass="tb tb-m" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">電傳</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_FAX_T" CssClass="tb tb-m" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">負責人</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtVEN_BOSS_T" CssClass="tb tb-s" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                            </table>
                            <br>
                            <table class="table table-bordered text-left">
                                <tr>
                                    <td style="width:30%"><asp:Label CssClass="control-label" runat="server">國外廠商授權在台辦理/代表效期</asp:Label></td>
                                    <td class="text-left" style="width:70%">
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
                                    <td><asp:Label CssClass="control-label" runat="server">授權範圍(權限)</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtAUTH_RANGE" CssClass="tb tb-m" runat="server" Text="在台代理推介產品">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">代理項品(產品)</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtAGENT_ITEM" CssClass="tb tb-l" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">核定在台代理/代表效期</asp:Label></td>
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
                                    <td><asp:Label CssClass="control-label" runat="server">附註事項</asp:Label></td>
                                    <td class="text-left">
                                        <asp:TextBox ID="txtOVC_MEMO" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server" Text="本文件僅供於授權有效期限及授權範圍內，從事產品推介業務，不得作為投標、議價、簽約之依據。"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer" style="text-align: center;">
                    <!--網頁尾-->
                    <asp:Button ID="btnNew" cssclass="btn-warning btnw2" runat="server" Text="新增" OnClick="btnNew_Click" />
                    &emsp;
                    <asp:Button ID="btnReset" cssclass="btn-default btnw2" runat="server" Text="取消" />
                </footer>
            </section>
        </div>
    </div>
</asp:Content>