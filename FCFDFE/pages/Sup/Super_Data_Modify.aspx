<%@ Page Title="" Language="C#" MasterPageFile="~/Super.Master" AutoEventWireup="true" CodeBehind="Super_Data_Modify.aspx.cs" Inherits="FCFDFE.pages.Sup.Super_Data_Modify" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<div class="row">
        <div style="width: 1000px; margin: auto;">
            <%--<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                <ContentTemplate>--%>
            <section class="panel">
                <header class="title">
                    登入限制修改頁面
                      <%--<div class="floating-menu" >
                      <h3>狀態確認欄</h3>
                      </div>--%>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <%--<nav class='floating-menu'> <h3>浮動標題</h3> <a>內文測試</a> </nav>--%>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:Panel ID="PnMessage_Account" runat="server"></asp:Panel>
                            <table class="table table-bordered">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server" >登入限制次數</asp:Label>
                                        <asp:TextBox CssClass="tb tb-s " ID="Loginmax_Text" Text=""  runat="server"  />
                                        <asp:Button CssClass="btn-warning" runat="server" Text="修改/存檔" OnClick="Loginmax_Save" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                       <asp:Label CssClass="control-label" runat="server">登入密碼：</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox CssClass="tb tb-s " ID="Password_Text" Text=""  runat="server"></asp:TextBox>
                                       <asp:Button CssClass="btn-warning" runat="server" Text="修改" OnClick="Pwd_change"/>
                                    </td>
                                    </tr>
                                <tr>
                                    <td id="Pwd_modify" runat="server" visible="false">
                                        <asp:Label CssClass="control-label" runat="server">新密碼：</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox CssClass="tb tb-s " ID="NewPwd_Text" Text="" TextMode="Password" runat="server"></asp:TextBox>

                                         <asp:RegularExpressionValidator ID="CHECK_PWD"
                                            runat="server"
                                            ErrorMessage="至少8碼與英數"
                                            Forecolor="Red"
                                            ControlToValidate="NewPwd_Text"
                                            ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-zA-Z]).*$">
                                        </asp:RegularExpressionValidator>

                                        <asp:Label CssClass="control-label" runat="server">再次輸入密碼：</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox CssClass="tb tb-s " ID="NewPwdCon_Text" Text="" TextMode="Password" runat="server"></asp:TextBox>

                                        <asp:RegularExpressionValidator ID="CHECK_PwdAGAIN"
                                            runat="server"
                                            ErrorMessage="至少8碼與英數"
                                            Forecolor="Red"
                                            ControlToValidate="NewPwdCon_Text"
                                            ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-zA-Z]).*$">
                                        </asp:RegularExpressionValidator>

                                        <asp:Button CssClass="btn-warning" runat="server" Text="存檔" OnClick="Pwd_Save" />
                                    </td>

                                </tr>
                                <tr>
                                    <td colspan="2">
                                       <asp:Label CssClass="control-label" TextMode="Password" runat="server">伺服器密碼：</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox CssClass="tb tb-s " ID="SeverPwd_Text"   runat="server"></asp:TextBox>
                                       <asp:Button CssClass="btn-warning" runat="server" Text="修改" OnClick="SerPwd_change" />
                                    </td>
                                     
                                </tr>
                               <tr>
                                   <td id="SerPwd_modify" runat="server" visible="false">
                                        <asp:Label CssClass="control-label" runat="server">新密碼：</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox CssClass="tb tb-s " ID="NewSerPwd_Text" Text="" TextMode="Password" runat="server"></asp:TextBox>

                                       <asp:RegularExpressionValidator ID="CHECK_SERPWD"
                                            runat="server"
                                            ErrorMessage="至少8碼與英數"
                                            Forecolor="Red"
                                            ControlToValidate="NewSerPwd_Text"
                                            ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-zA-Z]).*$">
                                        </asp:RegularExpressionValidator>

                                        <asp:Label CssClass="control-label" runat="server">再次輸入密碼：</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox CssClass="tb tb-s " ID="NewSerPwdCon_Text" Text="" TextMode="Password" runat="server"></asp:TextBox>

                                        <asp:RegularExpressionValidator ID="CHECK_SERPWDAGAIN"
                                            runat="server"
                                            ErrorMessage="至少8碼與英數"
                                            Forecolor="Red"
                                            ControlToValidate="NewSerPwdCon_Text"
                                            ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-zA-Z]).*$">
                                        </asp:RegularExpressionValidator>

                                       <asp:Button CssClass="btn-warning" runat="server" Text="存檔" OnClick="SerPwd_Save" />
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
