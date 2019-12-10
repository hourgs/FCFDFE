<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="FCFDFE.login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>國軍採購管理資訊系統</title><!--請自行更改標題-->
    <meta charset="utf-8" />
    <meta http-equiv="x-ua-compatible" content="IE=9" />
    <meta name="description" content="" /><!--請自行更改描述-->
    <meta name="keywords" content="" /><!--請自行更改最大關鍵字-->
    <meta name="viewport" content="width=device-width,initial-scale=1,maximum-scale=1" />
    <link href="~/assets/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="~/assets/css/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="containerIndex">
        <nav></nav>
        <asp:Literal ID="MSG" runat="server"></asp:Literal>
        <header>
            <img src="~/images/GM/logo.png" runat="server" alt="" width="150" /><span class="headerFont">國軍採購資訊系統</span>

        </header>
        <article>
            <div class="containerArticle">
                <div class="articleFont1">*請輸入您的帳號(身分證字號)及密碼</div>
                    <span class="articleFont2">
                        <label>帳號 </label>
                        <asp:TextBox ID="LoginName" class="text_input" title="請輸入帳號~!" runat="server" Style="width: 138px;"></asp:TextBox><!--請自行更改最大字元數-->
                    </span><br />
                    <span class="articleFont2">
                        <label>密碼 </label>
                        <asp:TextBox ID="LoginPass" runat="server" class="text_input" title="請輸入密碼~!" TextMode="Password" Style="width: 138px;"></asp:TextBox><!--請自行更改最大字元數-->
                    </span><br />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <img src="~/images/GM/key.png" runat="server" alt="" width="30" />
                    <asp:Button ID="Button1" runat="server" Text="登入" OnClick="btnlogin_Click1" />
                    &nbsp;
                    <asp:Button ID="btnforgotpass" runat="server" OnClick="btnforgotpass_Click" Text="忘記密碼"/>
                    <div class="articleFont3">●請先登入帳號密碼，進行系統認證與授權。</div>
                    <div class="articleFont3">●非使用者請點選右上方【帳號申請】，申請帳號權限。</div>
                <div class="imgApply"><a href="<%=ResolveClientUrl("~/pages/GM/AccountApplication")%>" onclick="handler();"><img src="~/images/GM/apply.png" runat="server" width="120" /></a></div>

            </div>
        </article>
        
        <footer></footer>
    </div>
    </form>
</body>

</html>

<script language="JavaScript" type="text/javascript"><!--

    // The Central Randomizer 1.3 (C) 1997 by Paul Houle (houle@msc.cornell.edu) 

    // See: http://www.msc.cornell.edu/~houle/javascript/randomizer.html 

    rnd.today = new Date();

    rnd.seed = rnd.today.getTime();

    function rnd() {
        rnd.seed = (rnd.seed * 9301 + 49297) % 233280;
        return rnd.seed / (233280.0);

    };

    function rand(number) {
        return Math.ceil(rnd() * number);

    };

    // end central randomizer. --> 



</script>

<script language="javascript" type="text/javascript">

    if (document.getElementById("code_op") != null) {
        ChangeCodeImg();

    }
    else {
        document.getElementById("Button1").disabled = false;
    }
    function ChangeCodeImg() {
        a = document.getElementById("ImageCheck");
        a.src = "inc/CodeImg.aspx?" + rand(10000000);
        document.getElementById("Button1").disabled = true;
    }

    function Open_Submit() {
        document.getElementById("Button1").disabled = "";
    }

    if (top != self) {
        top.location.href = "login.aspx";
    }
    //alert(navigator.appVersion);
    if (navigator.appVersion.indexOf("MSIE") == -1) {
        //alert("提醒 : 本管理系統建議議採用Internet Explorer 5.5 (或以上版本) 的瀏覽器。請開啟瀏覽器的 Cookies 與 JavaScript 功能。");   
    }

    function handler() {
        <%Session["IsLogined"] = "Apply";%>

    }

</script>