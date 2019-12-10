<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountApplication.aspx.cs" Inherits="FCFDFE.pages.GM.AccountApplication" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta name="description" content=""/>
    <meta name="author" content="Mosaddek"/>
    <meta name="keyword" content="FlatLab, Dashboard, Bootstrap, Admin, Template, Theme, Responsive, Fluid, Retina"/>
    <%--<link rel="shortcut icon" href="img/favicon.html">--%>

    <title>國防部</title>

    <!-- Bootstrap core CSS -->
    <link href="~/assets/css/bootstrap.css" rel="stylesheet"/>
    <link href="~/assets/css/bootstrap-reset.css" rel="stylesheet"/>
    <!--external css-->
    <link href="~/assets/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="~/assets/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="~/assets/css/owl.carousel.css" rel="stylesheet" type="text/css"/>
    <!--picker-->
    <link rel="stylesheet" type="text/css" href="~/assets/assets/bootstrap-datepicker/css/datepicker.css" />
    <link rel="stylesheet" type="text/css" href="~/assets/assets/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css" />
    <link rel="stylesheet" type="text/css" href="~/assets/assets/bootstrap-colorpicker/css/colorpicker.css" />
    <link rel="stylesheet" type="text/css" href="~/assets/assets/bootstrap-daterangepicker/daterangepicker.css" />
    <!-- Custom styles for this template -->
    <link href="~/assets/css/style.css" rel="stylesheet"/>
    <link href="~/assets/css/style-responsive.css" rel="stylesheet" />
    
    <!-- js placed at the end of the document so the pages load faster -->
    <%--<script src="<%=ResolveClientUrl("~/assets/js/jquery.js")%>"></script>--%>
    <%--<script src="<%=ResolveClientUrl("~/assets/js/jquery-1.8.3.min.js")%>"></script>--%>
    <script src="<%=ResolveClientUrl("~/assets/js/jquery-3.2.1.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/assets/js/bootstrap.min.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/assets/js/jquery.scrollTo.min.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/assets/js/jquery.nicescroll.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveClientUrl("~/assets/js/jquery.sparkline.js")%>" type="text/javascript"></script>
    <script src="<%=ResolveClientUrl("~/assets/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/assets/js/owl.carousel.js")%>" ></script>
    <script src="<%=ResolveClientUrl("~/assets/js/jquery.customSelect.min.js")%>" ></script>

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 tooltipss and media queries -->
    <!--[if lt IE 9]>
      <script src="~/assets/js/html5shiv.js"></script>
      <script src="~/assets/js/respond.min.js"></script>
    <![endif]-->
</head>
<body>
<form id="form1" runat="server">
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    帳號申請頁面
                    <nav class="floating-menu" id="floatmenu" visible="false" runat="server">
                        <h3>狀態確認欄</h3>
                    </nav>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
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
                                        <asp:TextBox id="MainContent_txtOVC_DEPT_CDE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <input type="button" value="單位查詢" class="btn-success" onclick="OpenWindow('txtOVC_DEPT_CDE', 'txtOVC_ONNAME')" />
                                        <asp:TextBox id="MainContent_txtOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">採購購案編號第一組代字：</asp:Label>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtPURCHASE_1" Text="99" CssClass="tb tb-s " runat="server"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label text-red" runat="server">輸入如：「TA」「PA」，若有多個，請以「，」間隔，如「TA，PA，EA」之方式。此欄位僅於採購管理系統使用，若您非採購管理人員，請輸入預設代碼"99"。</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">E-mail（國軍電子郵件信箱 webemail ）：</asp:Label>&nbsp;&nbsp;
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
                                        <asp:Label CssClass="control-label text-red" runat="server">（請輸入您的身分證字號）</asp:Label>
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
                                <asp:Button CssClass="btn-danger" CausesValidation="false" runat="server" Text="清除" OnClick="btnClear_Click"/>
                                <a class="btn-success" href="<%=ResolveClientUrl("~/login")%>">回登入頁面</a>
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="panel-footer text-center">
                    <!--網頁尾-->
                </footer>
            </section>
            <section id="pnAuth" class="panel" visible="false" runat="server">
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
            <section id="pnApply" class="panel" visible="false" runat="server">
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
                                        <asp:DropDownList ID="drpC_SN_SYS" CssClass="tb tb-m" OnSelectedIndexChanged="drpC_SN_SYS_SelectedIndexChanged" AutoPostBack="True" runat="server"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpC_SN_ROLE" CssClass="tb tb-m" OnSelectedIndexChanged="drpC_SN_ROLE_SelectedIndexChanged" AutoPostBack="True" runat="server"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpC_SN_AUTH" CssClass="tb tb-s" OnSelectedIndexChanged="drpC_SN_AUTH_SelectedIndexChanged" AutoPostBack="True" runat="server"></asp:DropDownList>
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
                                        <asp:Button Text="申請" CssClass="btn-success" OnClick="btnAuth_Click" CausesValidation="false" runat="server" />
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
</form>
    
    <!--custom switch-->
    <script src="<%=ResolveClientUrl("~/assets/js/bootstrap-switch.js")%>"></script>
    <!--custom tagsinput-->
    <script src="<%=ResolveClientUrl("~/assets/js/jquery.tagsinput.js")%>"></script>
    <!--custom checkbox & radio-->
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/js/ga.js")%>"></script>
    <!--picker-->
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/assets/bootstrap-datepicker/js/bootstrap-datepicker.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/assets/bootstrap-datetimepicker/js/bootstrap-datetimepicker.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/assets/bootstrap-daterangepicker/date.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/assets/bootstrap-daterangepicker/daterangepicker.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/assets/bootstrap-colorpicker/js/bootstrap-colorpicker.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/assets/ckeditor/ckeditor.js")%>"></script>
    <!--dataTable-->
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/assets/data-tables/jquery.dataTables.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/assets/data-tables/DT_bootstrap.js")%>"></script>
    <!--fileinput-->
    <script type="text/javascript" src="<%=ResolveClientUrl("~/assets/js/bootstrap-fileinput.js")%>"></script>

    <!--common script for all pages-->
    <script src="<%=ResolveClientUrl("~/assets/js/common-scripts.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/assets/js/common.js")%>"></script>
    <!--script for this page only-->
    <script src="<%=ResolveClientUrl("~/assets/js/dynamic-table.js")%>"></script>
    <!--script for this page-->
    <script src="<%=ResolveClientUrl("~/assets/js/sparkline-chart.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/assets/js/easy-pie-chart.js")%>"></script>
    <script src="<%=ResolveClientUrl("~/assets/js/form-component.js")%>"></script>

    <script>
        //custom select box
        $(function () {
            $('select.styled').customSelect();
        });
    </script>
    
    <script>
        function OpenWindow(CDE, NAME) {
            var win_width = 600;
            var win_height = 300;
            var PosX = (screen.width - win_width) / 2;
            var PosY = (screen.Height - win_height) / 2;
            features = "width=" + win_width + ",height=" + win_height + ",top=" + PosY + ",left=" + PosX;

            var encodeCDE = window.btoa(CDE);
            var encodeNAME = window.btoa(NAME);
            var theURL = '<%=ResolveClientUrl("~/Content/unitQuery.aspx?CDE=")%>' + encodeCDE + '&NAME=' + encodeNAME;
            var newwin = window.open(theURL, 'unitQuery', features);
        }
    </script>
</body>
</html>
