<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccountModify.aspx.cs" Inherits="FCFDFE.pages.GM.AccountModify" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });

        function onlyNum() {
            if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)))
                //考慮小鍵盤上的數字鍵
                event.returnvalue = false;
        }
    </script>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <%--<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                <ContentTemplate>--%>
            <section class="panel">
                <header class="title">
                    帳號修改頁面
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
                                        <asp:Label CssClass="control-label" runat="server">使用者姓名：</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtUSER_NAME" CssClass="tb tb-s " runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label text-red" runat="server">(若是單名，請於姓與名之間加入一個全形空白)</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">電話(軍線)：</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtIUSER_PHONE" CssClass="tb tb-s " runat="server" MaxLength="6" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" ></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="passrg"
                                            runat="server"
                                            ErrorMessage="至少六碼，且只能夠是數字"
                                            Forecolor="Red"
                                            ControlToValidate="txtIUSER_PHONE"
                                            ValidationExpression="^.*(?=.{6,})(?=.*\d)(?!.*[^\x00-\xff]).*$">
                                        </asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="valIuserPhone" 
                                          runat="server" 
                                          ControlToValidate="txtIUSER_PHONE" 
                                          Forecolor="Red"
                                          ErrorMessage="必填">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">單位代碼：</asp:Label>
                                        <asp:TextBox id="txtOVC_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <input type="button" value="單位查詢" class="btn-success" onclick="OpenWindow('txtOVC_DEPT_CDE', 'txtOVC_ONNAME')" />
                                        <asp:TextBox id="txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">採購購案編號第一組代字：</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtPURCHASE_1" CssClass="tb tb-s " runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label text-red" runat="server">輸入如:「TA」「PA」，若有多個，請以「，」間隔，如「TA，PA，EA」之方式。</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">E-mail(國軍電子郵件信箱webemail)：</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtEMAIL_ACCOUNT" CssClass="tb tb-l " runat="server"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="非E-MAIL 格式"
                                            Forecolor="Red"
                                            ControlToValidate="txtEMAIL_ACCOUNT"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                        </asp:RegularExpressionValidator>
                                        <%--<asp:RequiredFieldValidator ID="valEmailAccount" 
                                          runat="server" 
                                          ControlToValidate="txtEMAIL_ACCOUNT" 
                                          Forecolor="Red"
                                          ErrorMessage="必填">
                                        </asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">使用者帳號：</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtUSER_ID" CssClass="tb tb-m " runat="server" MaxLength="10"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label text-red" runat="server">(請輸入您的身分證字號)</asp:Label>
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                                            runat="server"
                                            ErrorMessage="輸入錯誤"
                                            Forecolor="Red"
                                            ControlToValidate="txtUSER_ID"
                                            ValidationExpression="^.*(?=.{10,})(A-Z0-9)+.*$">
                                        </asp:RegularExpressionValidator>--%>
                                        <%--<asp:RequiredFieldValidator ID="valUSER_ID" 
                                          runat="server" 
                                          ControlToValidate="txtUSER_ID" 
                                          Forecolor="Red"
                                          ErrorMessage="必填">
                                        </asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">密碼設定：</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtPWD" CssClass="tb tb-m text-password" runat="server"></asp:TextBox><br />
                                        <asp:Label CssClass="control-label text-red" runat="server">為符合資安政策請採英文大小寫及數字混和，最小長度8碼</asp:Label>
                                        <asp:RegularExpressionValidator ID="CHECK_ACCOUNT_PWD"
                                            runat="server"
                                            ErrorMessage="至少8碼與英數"
                                            Forecolor="Red"
                                            ControlToValidate="txtPWD"
                                            ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-zA-Z]).*$">
                                        </asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="valPwd" 
                                          runat="server" 
                                          ControlToValidate="txtPWD" 
                                          Forecolor="Red"
                                          ErrorMessage="必填">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">再次輸入密碼：</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtPWDConfirm" CssClass="tb tb-m text-password" runat="server"></asp:TextBox><br />
                                        <asp:Label CssClass="control-label text-red" runat="server">為符合資安政策請採英文大小寫及數字混和，最小長度8碼</asp:Label>
                                        <asp:RegularExpressionValidator ID="CHECK_ACCOUNT_AGAIN"
                                            runat="server"
                                            ErrorMessage="至少8碼與英數"
                                            Forecolor="Red"
                                            ControlToValidate="txtPWDConfirm"
                                            ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-zA-Z]).*$">
                                        </asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="valPwdConfirm" 
                                          runat="server" 
                                          ControlToValidate="txtPWDConfirm" 
                                          Forecolor="Red"
                                          ErrorMessage="必填">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                            <div class="text-center">
                                <asp:Button CssClass="btn-warning" runat="server" Text="確認" OnClick="btnSave_Click"/>
                                <%--<asp:Button ID="btnDel" CssClass="btn-danger" runat="server" Text="刪除" OnClientClick="reset()" />--%>
                                <asp:Button CssClass="btn-danger" CausesValidation="false" runat="server" Text="清除" OnClick="btnClear_Click"/>
                                <asp:Button CssClass="btn-success" CausesValidation="false" runat="server" Text="回上一頁" />
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                </footer>
            </section>
            <section class="panel">
                <header class="title">
                    系統使用權限資料
                </header>
                <asp:Panel ID="Panel2" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:GridView ID="GV_ACCOUNT_AUTH" DataKeyNames="AA_SN" CssClass="table data-table table-striped border-top table-bordered" AutoGenerateColumns="false" OnPreRender="GV_ACCOUNT_AUTH_PreRender" OnRowDataBound="GV_ACCOUNT_AUTH_RowDataBound" runat="server">
                                <Columns>
                                    <%--經反應權限、角色名稱對調--%>
                                    <asp:BoundField HeaderText="系統別" DataField="C_SN_SYS" />
                                    <asp:BoundField HeaderText="使用者角色" DataField="C_SN_ROLE" />
                                    <asp:BoundField HeaderText="使用者權限" DataField="C_SN_AUTH" />
                                    <asp:BoundField HeaderText="隸屬單位" DataField="C_SN_SUB" />
                                    <asp:BoundField HeaderText="上傳功能" DataField="IS_UPLOAD" />
                                    <asp:BoundField HeaderText="開放使用" DataField="IS_ENABLE" />
                                    <asp:BoundField HeaderText="處理狀態" DataField="IS_PRO" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                </footer>
            </section>
            <section class="panel">
                <header class="title">
                    系統使用權限申請
                </header>
                <asp:Panel ID="Panel1" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <asp:Panel ID="PnMessage_AccountAuth" runat="server"></asp:Panel>
                            <table class="table table-bordered">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">系統別</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">使用者角色</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">使用者權限</asp:Label>
                                    </td>
                                    <td id="tdTitleC_SN_SUB" runat="server">
                                        <asp:Label CssClass="control-label" runat="server">隸屬單位</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">上傳功能</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">開放使用</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">作業</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="drpC_SN_SYS" CssClass="tb tb-m" runat="server" OnSelectedIndexChanged="drpC_SN_SYS_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpC_SN_ROLE" CssClass="tb tb-m" runat="server" OnSelectedIndexChanged="drpC_SN_ROLE_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpC_SN_AUTH" CssClass="tb tb-s" runat="server" OnSelectedIndexChanged="drpC_SN_AUTH_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                    </td>
                                    <td id="tdC_SN_SUB" runat="server">
                                        <asp:DropDownList ID="drpC_SN_SUB" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpIS_UPLOAD" CssClass="tb tb-xs" runat="server"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" Text="否" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button CssClass="btn-success" runat="server" Text="申請" CausesValidation="false" OnClick="btnAuth_Click" />
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
                    <%--</ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnDel" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnLast" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>
    </div>
</asp:Content>