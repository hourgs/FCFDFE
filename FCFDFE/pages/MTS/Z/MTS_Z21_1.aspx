<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MTS_Z21_1.aspx.cs" Inherits="FCFDFE.pages.MTS.Z.MTS_Z21_1" %>
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
                    帳號管理-修改<br /><br />
                    <asp:Button ID="btnModify" cssclass="btn-success btnw4" runat="server" Text="修改頁" />
                    <asp:Button ID="btnAdd" cssclass="btn-warning btnw4" runat="server" Text="新增頁" />                    
                    <asp:Button ID="btnQuery" cssclass="btn-success btnw4" runat="server" Text="查詢頁" />
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered">              
                                <tr>
                                    <td style="width:1000px;" class="text-center" colspan="4">
                                        <asp:Label CssClass="control-label" runat="server">基本資料</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">帳號 - 姓名</asp:Label>
                                    </td>
                                    <td style="width:300px;">
                                       <asp:DropDownList  ID="drpAcId" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">使用狀態</asp:Label>
                                    </td>
                                    <td style="width:300px;">
                                       <asp:DropDownList  ID="drpAccountStatus" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:DropDownList>&nbsp;&nbsp;
                                        <asp:CheckBox ID="chkOdtReturnDate" CssClass="radioButton" runat="server" text="啟用後寄出告知信"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">姓名</asp:Label>&nbsp;
                                        <asp:Label CssClass="control-label text-red" runat="server">*</asp:Label>
                                    </td>
                                    <td style="width:300px;">
                                       <asp:TextBox ID="txtUserName" CssClass="tb tb-s " runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">描述</asp:Label>
                                    </td>
                                    <td style="width:300px;">
                                       <asp:TextBox ID="txtDescribe" CssClass="tb tb-m " runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">電話</asp:Label>
                                    </td>
                                    <td style="width:300px;">
                                       <asp:TextBox ID="txtIuserPhone" CssClass="tb tb-s " runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">單位</asp:Label>&nbsp;
                                        <asp:Label CssClass="control-label text-red" runat="server">*</asp:Label>
                                    </td>
                                    <td style="width:300px;">
                                        <asp:TextBox ID="txtDeptSn" CssClass="tb tb-s" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Button ID="btnDeptSn" cssclass="btn-success btnw2" runat="server" Text="單位" />&nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" cssclass="btn-default btnw4" runat="server" Text="資料清空" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">E-mail</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:TextBox ID="txtEmailAccount" CssClass="tb tb-l " runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">密碼</asp:Label>&nbsp;
                                        <asp:Label CssClass="control-label text-red" runat="server">*</asp:Label>
                                    </td>
                                    <td style="width:300px;">
                                        <asp:TextBox ID="txtPwd" CssClass="tb tb-s " runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Button ID="btnPwd" cssclass="btn-success btnw2" runat="server" Text="P" />&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server" Text="pwdLabel"></asp:Label>

                                    </td>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">確認密碼</asp:Label>&nbsp;
                                        <asp:Label CssClass="control-label text-red" runat="server">*</asp:Label>
                                    </td>
                                    <td style="width:300px;">
                                        <asp:TextBox ID="txtPwdConfirm" CssClass="tb tb-s " runat="server"></asp:TextBox>
                                        <asp:CheckBox ID="chkPwd" CssClass="radioButton text-red" runat="server" text="更新密碼"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:1000px;" class="text-center" colspan="4">
                                        <asp:Label CssClass="control-label text-red" runat="server">使用LDAP登入後，使用者資料將會與LDAP同步，</asp:Label><br />
                                        <asp:Label CssClass="control-label text-red" runat="server">為避免資料不一致，請至 軍備管理系統目錄服務 更改個人資料</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:1000px;" class="text-center" colspan="4">
                                        <asp:Label CssClass="control-label" runat="server">權限</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:200px;" class="text-center">
                                        <asp:Label CssClass="control-label" runat="server">角色</asp:Label>
                                    </td>
                                    <td style="width:800px;" colspan="3">
                                        <asp:CheckBoxList ID="chkCSnRole" CssClass="radioButton position-left"  RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem>內容1</asp:ListItem>
                                            <asp:ListItem>內容1</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align:center;">
                                <asp:Button ID="btnSave" cssclass="btn-warning" runat="server" Text="更新" />
                                <asp:Button ID="btnDel" cssclass="btn-danger" runat="server" Text="刪除" /><br /><br /> 
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
