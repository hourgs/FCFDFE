<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="FCFDFE.pages.GM.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!--Master原始程式碼-->
    <%--<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%: Page.Title %> - 我的 ASP.NET  應用程式</title>

    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>
    <webopt:bundlereference runat="server" path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />--%>
    
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="Mosaddek">
    <meta name="keyword" content="FlatLab, Dashboard, Bootstrap, Admin, Template, Theme, Responsive, Fluid, Retina">
    <%--<link rel="shortcut icon" href="img/favicon.html">--%>

    <title>國防部</title>

    <!-- Bootstrap core CSS -->
    <link href="~/assets/css/bootstrap.css" rel="stylesheet">
    <link href="~/assets/css/bootstrap-reset.css" rel="stylesheet">
    <!--external css-->
    <link href="~/assets/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="~/assets/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="~/assets/css/owl.carousel.css" rel="stylesheet" type="text/css">
    <!-- Custom styles for this template -->
    <link href="~/assets/css/style.css" rel="stylesheet">
    <link href="~/assets/css/style-responsive.css" rel="stylesheet" />

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 tooltipss and media queries -->
    <!--[if lt IE 9]>
      <script src="js/html5shiv.js"></script>
      <script src="js/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <form id="form1" runat="server">
        
            <div style="width: 1000px;margin-left:auto;margin-right:auto;margin-top:4em;">
                <header class="panel-heading" style="text-align:center; font-size:50px; font-weight: bold; color: 	#444444;font-family:微軟正黑體;">
                    <img style="width:10%;"src="<%=ResolveClientUrl("~/assets/img/loginicon.png")%>"/>國軍採購資訊系統
                </header>
                <div style="background-color: rgba(255, 255, 255, .5); width:55%;margin-left:auto;margin-right:auto;margin-top:1em;border:2px solid #AAAAAA;">
                    <div style="margin:1em;text-align:center;">
                        <br/>
                        <asp:Label ID="lblIdPwdInput" runat="server" Text="*請輸入您的帳號(身分證字號)及密碼" ForeColor="#005EEA" Font-Size="18px" Font-Bold="True" Font-Names="微軟正黑體"></asp:Label>
                          <table style="margin:auto;line-height:4em;letter-spacing:3px;">
                              <tr>
                                  <td>
                                      <asp:Label ID="lblAccount" runat="server" Text="帳號" Font-Size="22px" Font-Names="微軟正黑體" Font-Bold="True"></asp:Label>

                                  &nbsp;&nbsp;

                                  </td>
                                  <td>
                                      <asp:TextBox ID="txtAccount" runat="server" Height="28px" Width="200px"></asp:TextBox>
                                  </td>
                              </tr>
                              <tr>
                                  <td>
                                      <asp:Label ID="lblPwd" runat="server" Text="密碼" Font-Size="22px" Font-Names="微軟正黑體" Font-Bold="True"></asp:Label>

                                  &nbsp;&nbsp;

                                  </td>
                                  <td>
                                      <asp:TextBox ID="txtPwd" runat="server" Height="28px" OnTextChanged="TextBox2_TextChanged" Width="200px"></asp:TextBox>
                                  </td>
                              </tr>
                          </table>
                        <img style="width:3%;"src="<%=ResolveClientUrl("~/assets/img/loginkeyicon.png")%>"/>
                        &nbsp;&nbsp;<asp:Button ID="btnForPwd" CssClass="btn btn-danger" runat="server" Text="忘記密碼" />
                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnLogin" OnClick="btnLogin_Click" CssClass="btn btn-default" runat="server" Text="登入" /><br/><br/>
                        <table style="margin:auto;line-height:2em;letter-spacing:1px;">
                        <tr><td><asp:Label ID="lblDes1" runat="server" Text="●會員請登入帳號密碼，進行系統認證與授權。&nbsp;&nbsp;&nbsp;"></asp:Label></td></tr>
                        <tr><td><asp:Label ID="lblDes2" runat="server" Text="●非會員請點選下方[帳號申請]，申請帳號權限。"></asp:Label></td></tr>
                        </table>
                    </div>
                </div>
                <div style="width:1000px;text-align:center;">
                    <br/>
                    <asp:Button ID="btnAccApl" runat="server" Text="帳號申請" BackColor="#333333" ForeColor="White" BorderColor="#333333" BorderStyle="Solid" BorderWidth="5px" Font-Size="Small" Font-Bold="False" />
                </div>
            </div>
        
    </form>
</body>
</html>
