<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Deptadd.aspx.cs" Inherits="FCFDFE.pages.GM.Deptadd" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        th{
            background-color:turquoise;
            color:white;
            font-size:18px;
            font-weight:bold ;
        }
    </style>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered text-center">
                                <tr>
                                    <th colspan="6">
                                        <asp:Label CssClass="th_label" runat="server">全域組織單位－新增</asp:Label>
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">單位代碼</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">單位名稱</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_ONNAME" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">主官階級名稱</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_MANAGER" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">單位簡稱</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_DEPT_SEC_NAME" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="4">
                                        <asp:Label CssClass="control-label text-red" runat="server">（若為採購單位時，則本欄位將作為發文時之單位名稱）</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">部門地址</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtOVC_DEPT_MAILADDR" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">單位狀況</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_ENABLE" CssClass="tb tb-full" runat="server">
                                            <asp:ListItem Value="0">0：停用</asp:ListItem>
                                            <asp:ListItem Value="1">1：現用</asp:ListItem>
                                            <asp:ListItem Value="2">2：戰時</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">傳真號碼</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_DEPT_FAX" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">電話號碼</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_DEPT_PHONE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">單位郵政信箱</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_EMAILADDRESS" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="3">
                                        <asp:Label CssClass="control-label text-blue" runat="server">採購部分</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">上級單位代碼</asp:Label>
                                        <input type="button" value="代碼查詢" class="btn-success" onclick="OpenWindow('txtOVC_TOP_DEPT', 'txtOVC_TOP_DEPT2')" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_TOP_DEPT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-blue" runat="server">所屬之採購單位代碼</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_PURCHASE_DEPT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">單位類別</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_CLASS" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-blue" runat="server">所屬之採購單位主官職稱</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_MANAGER_TITLE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">發文字號</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_DEPT_DOC_NAME" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-blue" runat="server">是否為採購單位(Y/N)</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_PURCHASE_OK" CssClass="tb tb-m" runat="server">
                                            <asp:ListItem Value="Y">Y：是</asp:ListItem>
                                            <asp:ListItem Value="N">N：否</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">接轉部分</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-blue" runat="server">上級單位名稱</asp:Label>
                                        <%--<input type="button" value="代碼查詢" class="btn-success" onclick="OpenWindow('txtOVC_TOP_DEPT', 'txtOVC_TOP_DEPT2')" />--%>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOVC_TOP_DEPT2" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-blue" runat="server">單位類別</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_CLASS2" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Button CssClass="btn-success" OnClick="btnNew_Click" Text="新增組織單位" runat="server" />
                                        <asp:Button CssClass="btn-success" OnClick="btnBack_Click" Text="回上頁" runat="server" />
                                    </td>
                                </tr>
                            </table>
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
