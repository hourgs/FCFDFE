<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CIMS_C12.aspx.cs" Inherits="FCFDFE.pages.CIMS.C.CIMS_C12" %>

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
                    <div>工程會決標購案查詢功能</div>
                </header>
                <asp:Panel ID="Panel"  runat="server" >
                    <div class="panel-body">
                    <div class="form">
                        <div class="cmxform form-horizontal tasi-form">
                            <table class="table table-bordered text-left">
                                <tr>
                                    <td>
                                        <asp:Button ID="Button98" CssClass="btn-success btnw2" runat="server" Text="98舊資料" OnClick="Button98_Click" Width="100px"/></td>
                                    <td style="width: 40%;">
                                        <asp:Label CssClass="control-label" runat="server">98年以前公程會決標資料查詢</asp:Label></td>
                                    <td>
                                        <asp:Button ID="Button99" CssClass="btn-success btnw2" runat="server" Text="99年新資料" OnClick="Button99_Click" Width="100px"/></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">98年以後公程會決標資料查詢</asp:Label></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                </asp:Panel>
                
                <asp:Panel ID="PnMessage"  runat="server" ></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <asp:Panel ID="Panel1"  runat="server" Visible="False" >
                     <div class="panel-body">
                     <div class="form">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-left">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">採購案號</asp:Label>
                                    </td>
                                    <td style="width: 40%;">
                                        <asp:TextBox ID="txtovc_x" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">採購品項(標的名稱)</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtovc_w" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">決標日期區間</asp:Label></td>
                                    <td>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtovc_aq_s" CssClass="tb tb-s position-left text-change" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <%--<div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtovc_aq_s" CssClass="tb tb-s position-left text-change" runat="server" ReadOnly="true"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>--%>
                                        <asp:Label CssClass="control-label position-left" runat="server">至&ensp;</asp:Label>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtovc_aq_e" CssClass="tb tb-s position-left text-change" runat="server"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>
                                        <%--<div class="input-append date position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                            <asp:TextBox ID="txtovc_aq_e" CssClass="tb tb-s position-left text-change" runat="server" ReadOnly="true"></asp:TextBox>
                                            <div class="add-on"><i class="icon-calendar"></i></div>
                                        </div>--%>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">廠商名稱</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtovc_a" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">招標方式</asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="txtovc_aw" CssClass="tb tb-l" runat="server">
                                             <asp:ListItem value="" selected>請選擇</asp:ListItem>
                                            <asp:ListItem value="公開取得報價單或企劃書">公開取得報價單或企劃書</asp:ListItem>
                                            <asp:ListItem value="公開招標">公開招標</asp:ListItem>
                                            <asp:ListItem value="限制性招標(未經公開評選或公開徵求)">限制性招標(未經公開評選或公開徵求)</asp:ListItem>
                                            <asp:ListItem value="限制性招標(經公開評選或公開徵求)">限制性招標(經公開評選或公開徵求)</asp:ListItem>
                                            <asp:ListItem value="限制性招標未經公開評選或公開徵求">限制性招標未經公開評選或公開徵求</asp:ListItem>
                                            <asp:ListItem value="限制性招標經公開評選或公開徵求">限制性招標經公開評選或公開徵求</asp:ListItem>
                                            <asp:ListItem value="選擇性招標(建立合格廠商名單後續邀標)">選擇性招標(建立合格廠商名單後續邀標)</asp:ListItem>
                                            <asp:ListItem value="選擇性招標(個案)">選擇性招標(個案)</asp:ListItem>
                                            <asp:ListItem value="選擇性招標建立合格廠商名單後續邀標">選擇性招標建立合格廠商名單後續邀標</asp:ListItem>
                                            <asp:ListItem value="選擇性招標個案">選擇性招標個案</asp:ListItem>                                           
                                        </asp:DropDownList></td>

                                     <td>
                                        <asp:Label CssClass="control-label" runat="server">決標方式</asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="txtovc_bc" CssClass="tb tb-l" runat="server">
                                             <asp:ListItem value="" selected>請選擇</asp:ListItem>
                                            <asp:ListItem value="最有利標">最有利標</asp:ListItem>
                                            <asp:ListItem value="最低標">最低標</asp:ListItem>
                                            <asp:ListItem value="最高標">最高標</asp:ListItem>
                                        </asp:DropDownList></td>                
                                </tr>
                                <tr>
                                     <td>
                                        <asp:Label CssClass="control-label" runat="server">招標機關</asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="txtovc_y" CssClass="tb tb-l" runat="server">
                                            <asp:ListItem value="" selected>請選擇</asp:ListItem>
                                            <asp:ListItem value="三軍總醫院">三軍總醫院</asp:ListItem>
                                            <asp:ListItem value="三軍總醫院採購組一">三軍總醫院採購組一</asp:ListItem> 
                                            <asp:ListItem value="三軍總醫院澎湖院區">三軍總醫院澎湖院區</asp:ListItem> 
                                            <asp:ListItem value="中正國防幹部預備學校">中正國防幹部預備學校</asp:ListItem> 
                                            <asp:ListItem value="中科院一所">中科院一所</asp:ListItem> 
                                            <asp:ListItem value="中部地區後備司令部">中部地區後備司令部</asp:ListItem> 
                                            <asp:ListItem value="空軍作戰指揮部">空軍作戰指揮部</asp:ListItem> 
                                            <asp:ListItem value="空軍防空砲兵指揮部">空軍防空砲兵指揮部</asp:ListItem> 
                                            <asp:ListItem value="空軍松山基地指揮部">空軍松山基地指揮部</asp:ListItem> 
                                            <asp:ListItem value="空軍軍官學校">空軍軍官學校</asp:ListItem> 
                                            <asp:ListItem value="空軍氣象聯隊">空軍氣象聯隊</asp:ListItem> 
                                            <asp:ListItem value="空軍航空技術學校">空軍航空技術學校</asp:ListItem> 
                                            <asp:ListItem value="空軍航空技術學院">空軍航空技術學院</asp:ListItem> 
                                            <asp:ListItem value="空軍教育訓練暨準則發展指揮部">空軍教育訓練暨準則發展指揮部</asp:ListItem> 
                                            <asp:ListItem value="空軍第一後勤指揮部">空軍第一後勤指揮部</asp:ListItem> 
                                            <asp:ListItem value="空軍第七三七戰術戰鬥機聯隊">空軍第七三七戰術戰鬥機聯隊</asp:ListItem> 
                                            <asp:ListItem value="空軍第三後勤指揮部">空軍第三後勤指揮部</asp:ListItem> 
                                            <asp:ListItem value="空軍第四Ｏ一戰術混合聯隊">空軍第四Ｏ一戰術混合聯隊</asp:ListItem> 
                                            <asp:ListItem value="空軍第四九九戰術戰鬥機聯隊">空軍第四九九戰術戰鬥機聯隊</asp:ListItem> 
                                            <asp:ListItem value="空軍第四二七戰術戰鬥機聯隊">空軍第四二七戰術戰鬥機聯隊</asp:ListItem> 
                                            <asp:ListItem value="空軍第四三九混合聯隊">空軍第四三九混合聯隊</asp:ListItem> 
                                            <asp:ListItem value="空軍第四五五戰術戰鬥機聯隊">空軍第四五五戰術戰鬥機聯隊</asp:ListItem> 
                                            <asp:ListItem value="空軍第四四三戰術戰鬥機聯隊">空軍第四四三戰術戰鬥機聯隊</asp:ListItem> 
                                            <asp:ListItem value="空軍通信航管資訊聯隊">空軍通信航管資訊聯隊</asp:ListItem> 
                                            <asp:ListItem value="空軍戰術管制聯隊">空軍戰術管制聯隊</asp:ListItem> 
                                            <asp:ListItem value="南部地區後備指揮部">南部地區後備指揮部</asp:ListItem> 
                                            <asp:ListItem value="海軍一六八艦隊">海軍一六八艦隊</asp:ListItem> 
                                            <asp:ListItem value="海軍左營後勤支援指揮部">海軍左營後勤支援指揮部</asp:ListItem> 
                                            <asp:ListItem value="海軍軍官學校">海軍軍官學校</asp:ListItem> 
                                            <asp:ListItem value="海軍航空指揮部">海軍航空指揮部</asp:ListItem> 
                                            <asp:ListItem value="海軍馬公後勤支援指揮部">海軍馬公後勤支援指揮部</asp:ListItem> 
                                            <asp:ListItem value="海軍基隆後勤支援指揮部">海軍基隆後勤支援指揮部</asp:ListItem> 
                                            <asp:ListItem value="海軍教育訓練暨準則發展司令部">海軍教育訓練暨準則發展司令部</asp:ListItem> 
                                            <asp:ListItem value="海軍造船發展中心">海軍造船發展中心</asp:ListItem> 
                                            <asp:ListItem value="海軍陸戰隊指揮部">海軍陸戰隊指揮部</asp:ListItem> 
                                            <asp:ListItem value="海軍戰系工廠">海軍戰系工廠</asp:ListItem> 
                                            <asp:ListItem value="海軍營運中心">海軍營運中心</asp:ListItem> 
                                            <asp:ListItem value="海軍艦隊司令部">海軍艦隊司令部</asp:ListItem> 
                                            <asp:ListItem value="海軍蘇澳後勤支援指揮部供應處採購科">海軍蘇澳後勤支援指揮部供應處採購科</asp:ListItem> 
                                            <asp:ListItem value="國防大學">國防大學</asp:ListItem> 
                                            <asp:ListItem value="國防部">國防部</asp:ListItem> 
                                            <asp:ListItem value="國防部中山科學研究院高雄採購站">國防部中山科學研究院高雄採購站</asp:ListItem> 
                                            <asp:ListItem value="國防部空軍司令部">國防部空軍司令部</asp:ListItem> 
                                            <asp:ListItem value="國防部空軍司令部後勤處">國防部空軍司令部後勤處</asp:ListItem> 
                                            <asp:ListItem value="國防部青年日報社">國防部青年日報社</asp:ListItem> 
                                            <asp:ListItem value="國防部後備司令部">國防部後備司令部</asp:ListItem> 
                                            <asp:ListItem value="國防部後備司令部中部地區後備司令部">國防部後備司令部中部地區後備司令部</asp:ListItem> 
                                            <asp:ListItem value="國防部後備司令部北部地區後備司令部">國防部後備司令部北部地區後備司令部</asp:ListItem> 
                                            <asp:ListItem value="國防部後備司令部北部地區後備指揮部">國防部後備司令部北部地區後備指揮部</asp:ListItem> 
                                            <asp:ListItem value="國防部後備司令部南部地區後備司令部">國防部後備司令部南部地區後備司令部</asp:ListItem> 
                                            <asp:ListItem value="國防部政治作戰總隊">國防部政治作戰總隊</asp:ListItem> 
                                            <asp:ListItem value="國防部軍事安全總隊">國防部軍事安全總隊</asp:ListItem> 
                                            <asp:ListItem value="國防部軍事情報局">國防部軍事情報局</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局">國防部軍備局</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局工程營產中心">國防部軍備局工程營產中心</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局中山科學研究院">國防部軍備局中山科學研究院</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局中山科學研究院人事行政處">國防部軍備局中山科學研究院人事行政處</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局中山科學研究院系統發展中心">國防部軍備局中山科學研究院系統發展中心</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局中山科學研究院系統製造中心">國防部軍備局中山科學研究院系統製造中心</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局中山科學研究院第五研究所">國防部軍備局中山科學研究院第五研究所</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局中山科學研究院第四研究所">國防部軍備局中山科學研究院第四研究所</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局中山科學研究院設施供應處">國防部軍備局中山科學研究院設施供應處</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局中山科學研究院電子系統研究所">國防部軍備局中山科學研究院電子系統研究所</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局中科院第二研究所">國防部軍備局中科院第二研究所</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局中科院資訊通信研究所">國防部軍備局中科院資訊通信研究所</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局生產製造中心">國防部軍備局生產製造中心</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局生產製造中心第205廠">國防部軍備局生產製造中心第205廠</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局生產製造中心第二○二廠">國防部軍備局生產製造中心第二○二廠</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局生產製造中心第二○四廠">國防部軍備局生產製造中心第二○四廠</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局生產製造中心第二０三廠">國防部軍備局生產製造中心第二０三廠</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局生產製造中心第四０一廠">國防部軍備局生產製造中心第四０一廠</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局生產製造中心第四０二廠">國防部軍備局生產製造中心第四０二廠</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局採購中心">國防部軍備局採購中心</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局採購中心工程發包處">國防部軍備局採購中心工程發包處</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局採購中心駐美採購組">國防部軍備局採購中心駐美採購組</asp:ListItem> 
                                            <asp:ListItem value="國防部軍備局規格鑑測中心">國防部軍備局規格鑑測中心</asp:ListItem> 
                                            <asp:ListItem value="國防部軍醫局">國防部軍醫局</asp:ListItem> 
                                            <asp:ListItem value="國防部海軍司令部">國防部海軍司令部</asp:ListItem> 
                                            <asp:ListItem value="國防部海軍司令部後勤處">國防部海軍司令部後勤處</asp:ListItem> 
                                            <asp:ListItem value="國防部參謀本部飛彈指揮部">國防部參謀本部飛彈指揮部</asp:ListItem> 
                                            <asp:ListItem value="國防部參謀本部資電作戰指揮部">國防部參謀本部資電作戰指揮部</asp:ListItem> 
                                            <asp:ListItem value="國防部部長辦公室">國防部部長辦公室</asp:ListItem> 
                                            <asp:ListItem value="國防部陸軍司令部">國防部陸軍司令部</asp:ListItem> 
                                            <asp:ListItem value="國防部電訊發展室">國防部電訊發展室</asp:ListItem> 
                                            <asp:ListItem value="國防部福利總處">國防部福利總處</asp:ListItem> 
                                            <asp:ListItem value="國防部憲兵司令部">國防部憲兵司令部</asp:ListItem> 
                                            <asp:ListItem value="國防部總政治作戰局">國防部總政治作戰局</asp:ListItem> 
                                            <asp:ListItem value="國防部聯合後勤司令部">國防部聯合後勤司令部</asp:ListItem> 
                                            <asp:ListItem value="國防部聯合後勤司令部採購處">國防部聯合後勤司令部採購處</asp:ListItem> 
                                            <asp:ListItem value="國防醫學院">國防醫學院</asp:ListItem> 
                                            <asp:ListItem value="國防醫學院預防醫學研究所">國防醫學院預防醫學研究所</asp:ListItem> 
                                            <asp:ListItem value="國軍北投醫院">國軍北投醫院</asp:ListItem> 
                                            <asp:ListItem value="國軍台中總醫院">國軍台中總醫院</asp:ListItem> 
                                            <asp:ListItem value="國軍台中總醫院（行政組）">國軍台中總醫院（行政組）</asp:ListItem> 
                                            <asp:ListItem value="國軍左營總醫院">國軍左營總醫院</asp:ListItem> 
                                            <asp:ListItem value="國軍岡山醫院">國軍岡山醫院</asp:ListItem> 
                                            <asp:ListItem value="國軍松山總醫院">國軍松山總醫院</asp:ListItem> 
                                            <asp:ListItem value="國軍花蓮總醫院">國軍花蓮總醫院</asp:ListItem> 
                                            <asp:ListItem value="國軍桃園總醫院">國軍桃園總醫院</asp:ListItem> 
                                            <asp:ListItem value="國軍桃園總醫院國軍新竹地區醫院">國軍桃園總醫院國軍新竹地區醫院</asp:ListItem> 
                                            <asp:ListItem value="國軍桃園總醫院衛保組">國軍桃園總醫院衛保組</asp:ListItem> 
                                            <asp:ListItem value="國軍高雄總醫院">國軍高雄總醫院</asp:ListItem> 
                                            <asp:ListItem value="清泉崗高爾夫球場">清泉崗高爾夫球場</asp:ListItem> 
                                            <asp:ListItem value="陸軍司令部後勤處">陸軍司令部後勤處</asp:ListItem> 
                                            <asp:ListItem value="陸軍步兵訓練指揮部暨步兵學校">陸軍步兵訓練指揮部暨步兵學校</asp:ListItem> 
                                            <asp:ListItem value="陸軍花東防衛指揮部">陸軍花東防衛指揮部</asp:ListItem> 
                                            <asp:ListItem value="陸軍金門防衛指揮部">陸軍金門防衛指揮部</asp:ListItem> 
                                            <asp:ListItem value="陸軍保修指揮部飛彈光電基地勤務廠">陸軍保修指揮部飛彈光電基地勤務廠</asp:ListItem> 
                                            <asp:ListItem value="陸軍保修指揮部航空基地勤務廠">陸軍保修指揮部航空基地勤務廠</asp:ListItem> 
                                            <asp:ListItem value="陸軍軍官學校">陸軍軍官學校</asp:ListItem> 
                                            <asp:ListItem value="陸軍航空特戰指揮部">陸軍航空特戰指揮部</asp:ListItem> 
                                            <asp:ListItem value="陸軍馬祖防衛指揮部">陸軍馬祖防衛指揮部</asp:ListItem> 
                                            <asp:ListItem value="陸軍專科學校">陸軍專科學校</asp:ListItem> 
                                            <asp:ListItem value="陸軍第八軍團指揮部">陸軍第八軍團指揮部</asp:ListItem> 
                                            <asp:ListItem value="陸軍第十軍團">陸軍第十軍團</asp:ListItem> 
                                            <asp:ListItem value="陸軍第六軍團指揮部">陸軍第六軍團指揮部</asp:ListItem> 
                                            <asp:ListItem value="陸軍通信電子資訊學校">陸軍通信電子資訊學校</asp:ListItem> 
                                            <asp:ListItem value="陸軍機械化步兵第二九八旅">陸軍機械化步兵第二九八旅</asp:ListItem> 
                                            <asp:ListItem value="澎湖防衛指揮部">澎湖防衛指揮部</asp:ListItem> 
                                            <asp:ListItem value="憲兵204指揮部">憲兵204指揮部</asp:ListItem> 
                                            <asp:ListItem value="憲兵二０二指揮部">憲兵二０二指揮部</asp:ListItem> 
                                            <asp:ListItem value="憲兵二０五指揮部">憲兵二０五指揮部</asp:ListItem> 
                                            <asp:ListItem value="憲兵學校">憲兵學校</asp:ListItem> 
                                            <asp:ListItem value="聯合後勤學校">聯合後勤學校</asp:ListItem> 
                                            <asp:ListItem value="聯勤兵工整備發展中心">聯勤兵工整備發展中心</asp:ListItem> 
                                            <asp:ListItem value="聯勤兵工整備發展中心工務處工業室">聯勤兵工整備發展中心工務處工業室</asp:ListItem> 
                                            <asp:ListItem value="聯勤汽車基地勤務廠">聯勤汽車基地勤務廠</asp:ListItem> 
                                            <asp:ListItem value="聯勤金門地區支援指揮部">聯勤金門地區支援指揮部</asp:ListItem> 
                                            <asp:ListItem value="聯勤第一地區支援指揮部">聯勤第一地區支援指揮部</asp:ListItem> 
                                            <asp:ListItem value="聯勤第二地區支援指揮部">聯勤第二地區支援指揮部</asp:ListItem> 
                                            <asp:ListItem value="聯勤第三地區支援指揮部">聯勤第三地區支援指揮部</asp:ListItem> 
                                            <asp:ListItem value="聯勤第五地區支援指揮部">聯勤第五地區支援指揮部</asp:ListItem> 
                                            <asp:ListItem value="聯勤第四地區支援指揮部">聯勤第四地區支援指揮部</asp:ListItem> 
                                            <asp:ListItem value="聯勤通信電子器材基地勤務廠">聯勤通信電子器材基地勤務廠</asp:ListItem> 
                                            <asp:ListItem value="聯勤儲備中心">聯勤儲備中心</asp:ListItem> 
                                        </asp:DropDownList></td>
                                   <td>
                                        <asp:Label CssClass="control-label" runat="server">標的分類</asp:Label></td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="txtovc_at" CssClass="tb tb-l" runat="server">
                                            <asp:ListItem value="" selected>請選擇</asp:ListItem>
                                            <asp:ListItem value="工程類-水道、海港、水壩及其他水利工程">工程類-水道、海港、水壩及其他水利工程</asp:ListItem>
                                            <asp:ListItem value="工程類-水道海港水壩及其他水利工程">工程類-水道海港水壩及其他水利工程</asp:ListItem>
                                            <asp:ListItem value="工程類-水管及排水設施鋪設工程">工程類-水管及排水設施鋪設工程</asp:ListItem>
                                            <asp:ListItem value="工程類-瓦斯安裝工程">工程類-瓦斯安裝工程</asp:ListItem>
                                            <asp:ListItem value="工程類-地板及牆面貼磚工程">工程類-地板及牆面貼磚工程</asp:ListItem>
                                            <asp:ListItem value="工程類-地區性管線及電纜; 輔助性工程">工程類-地區性管線及電纜; 輔助性工程</asp:ListItem>
                                            <asp:ListItem value="工程類-快速道路(不含高架快速道路), 街道, 馬路, 鐵路及機場跑道">工程類-快速道路(不含高架快速道路), 街道, 馬路, 鐵路及機場跑道</asp:ListItem>
                                            <asp:ListItem value="工程類-快速道路不含高架快速道路 街道 馬路 鐵路及機場跑道">工程類-快速道路不含高架快速道路 街道 馬路 鐵路及機場跑道</asp:ListItem>
                                            <asp:ListItem value="工程類-其他土木工程">工程類-其他土木工程</asp:ListItem>
                                            <asp:ListItem value="工程類-其他用途建築工程">工程類-其他用途建築工程</asp:ListItem>
                                            <asp:ListItem value="工程類-其他安裝工程">工程類-其他安裝工程</asp:ListItem>
                                            <asp:ListItem value="工程類-其他專業工程">工程類-其他專業工程</asp:ListItem>
                                            <asp:ListItem value="工程類-其他裝修工程">工程類-其他裝修工程</asp:ListItem>
                                            <asp:ListItem value="工程類-其他鋪地板, 牆面及壁紙工程">工程類-其他鋪地板, 牆面及壁紙工程</asp:ListItem>
                                            <asp:ListItem value="工程類-拆除工程">工程類-拆除工程</asp:ListItem>
                                            <asp:ListItem value="工程類-油漆工程">工程類-油漆工程</asp:ListItem>
                                            <asp:ListItem value="工程類-長程管線, 通訊及電線(纜)">工程類-長程管線, 通訊及電線(纜)</asp:ListItem>
                                            <asp:ListItem value="工程類-室內裝潢工程">工程類-室內裝潢工程</asp:ListItem>
                                            <asp:ListItem value="工程類-屋頂及防水工程">工程類-屋頂及防水工程</asp:ListItem>
                                            <asp:ListItem value="工程類-玻璃裝潢及窗戶玻璃裝修工程">工程類-玻璃裝潢及窗戶玻璃裝修工程</asp:ListItem>
                                            <asp:ListItem value="工程類-倉儲及工業建築工程">工程類-倉儲及工業建築工程</asp:ListItem>
                                            <asp:ListItem value="工程類-粉刷工程">工程類-粉刷工程</asp:ListItem>
                                            <asp:ListItem value="工程類-基地整建及清理工程">工程類-基地整建及清理工程</asp:ListItem>
                                            <asp:ListItem value="工程類-基礎工程(含打樁)">工程類-基礎工程(含打樁)</asp:ListItem>
                                            <asp:ListItem value="工程類-教育用建築工程">工程類-教育用建築工程</asp:ListItem>
                                            <asp:ListItem value="工程類-混凝土工程">工程類-混凝土工程</asp:ListItem>
                                            <asp:ListItem value="工程類-單雙棟式住宅建築工程">工程類-單雙棟式住宅建築工程</asp:ListItem>
                                            <asp:ListItem value="工程類-圍籬及護欄工程">工程類-圍籬及護欄工程</asp:ListItem>
                                            <asp:ListItem value="工程類-絕緣工程(電線, 水, 熱, 聲)">工程類-絕緣工程(電線, 水, 熱, 聲)</asp:ListItem>
                                            <asp:ListItem value="工程類-開挖及土方工程">工程類-開挖及土方工程</asp:ListItem>
                                            <asp:ListItem value="工程類-暖氣 通風及空調工程">工程類-暖氣 通風及空調工程</asp:ListItem>
                                            <asp:ListItem value="工程類-暖氣, 通風及空調工程">工程類-暖氣, 通風及空調工程</asp:ListItem>
                                            <asp:ListItem value="工程類-電力工程">工程類-電力工程</asp:ListItem>
                                            <asp:ListItem value="財物類">財物類</asp:ListItem>
                                            <asp:ListItem value="財物類-工具機 其零件及附件">財物類-工具機 其零件及附件</asp:ListItem>
                                            <asp:ListItem value="財物類-工具機, 其零件及附件">財物類-工具機, 其零件及附件</asp:ListItem>
                                            <asp:ListItem value="財物類-引擎 渦輪及其零件">財物類-引擎 渦輪及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-引擎, 渦輪及其零件">財物類-引擎, 渦輪及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-木製品 軟木製品 草及編結材料產品">財物類-木製品 軟木製品 草及編結材料產品</asp:ListItem>
                                            <asp:ListItem value="財物類-木製品, 軟木製品, 草及編結材料產品">財物類-木製品, 軟木製品, 草及編結材料產品</asp:ListItem>
                                            <asp:ListItem value="財物類-水">財物類-水</asp:ListItem>
                                            <asp:ListItem value="財物類-牛奶品">財物類-牛奶品</asp:ListItem>
                                            <asp:ListItem value="財物類-未加工之銅 鎳 鋁 氧化鋁 鉛 鋅及錫">財物類-未加工之銅 鎳 鋁 氧化鋁 鉛 鋅及錫</asp:ListItem>
                                            <asp:ListItem value="財物類-未加工之銅, 鎳, 鋁, 氧化鋁, 鉛, 鋅及錫">財物類-未加工之銅, 鎳, 鋁, 氧化鋁, 鉛, 鋅及錫</asp:ListItem>
                                            <asp:ListItem value="財物類-未明列於其他章節之化學產品">財物類-未明列於其他章節之化學產品</asp:ListItem>
                                            <asp:ListItem value="財物類-皮革與皮革品 鞋靴">財物類-皮革與皮革品 鞋靴</asp:ListItem>
                                            <asp:ListItem value="財物類-皮革與皮革品, 鞋靴">財物類-皮革與皮革品, 鞋靴</asp:ListItem>
                                            <asp:ListItem value="財物類-石材 砂及泥土">財物類-石材 砂及泥土</asp:ListItem>
                                            <asp:ListItem value="財物類-石材, 砂及泥土">財物類-石材, 砂及泥土</asp:ListItem>
                                            <asp:ListItem value="財物類-石油及天然氣">財物類-石油及天然氣</asp:ListItem>
                                            <asp:ListItem value="財物類-光學儀器 攝影設備及其零件與附件">財物類-光學儀器 攝影設備及其零件與附件</asp:ListItem>
                                            <asp:ListItem value="財物類-光學儀器, 攝影設備及其零件與附件">財物類-光學儀器, 攝影設備及其零件與附件</asp:ListItem>
                                            <asp:ListItem value="財物類-吊車,操作處理設備及其零件">財物類-吊車,操作處理設備及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-吊車操作處理設備及其零件">財物類-吊車操作處理設備及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-成衣 毛皮服裝除外">財物類-成衣 毛皮服裝除外</asp:ListItem>
                                            <asp:ListItem value="財物類-成衣, 毛皮服裝除外">財物類-成衣, 毛皮服裝除外</asp:ListItem>
                                            <asp:ListItem value="財物類-收音機、電視, 通訊器材及儀器">財物類-收音機、電視, 通訊器材及儀器</asp:ListItem>
                                            <asp:ListItem value="財物類-收音機電視 通訊器材及儀器">財物類-收音機電視 通訊器材及儀器</asp:ListItem>
                                            <asp:ListItem value="財物類-肉類 魚 果實 蔬菜及油脂">財物類-肉類 魚 果實 蔬菜及油脂</asp:ListItem>
                                            <asp:ListItem value="財物類-肉類, 魚, 果實, 蔬菜,及油脂">財物類-肉類, 魚, 果實, 蔬菜,及油脂</asp:ListItem>
                                            <asp:ListItem value="財物類-冶金機具及其零件">財物類-冶金機具及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-其他非金屬礦物產品">財物類-其他非金屬礦物產品</asp:ListItem>
                                            <asp:ListItem value="財物類-其他非鐵金屬及其製品（包括廢料及碎料）；瓷金及其製品；含金屬或其化合物之灰末及渣滓（不包括煉製鋼鐵所產生者）">財物類-其他非鐵金屬及其製品（包括廢料及碎料）；瓷金及其製品；含金屬或其化合物之灰末及渣滓（不包括煉製鋼鐵所產生者）</asp:ListItem>
                                            <asp:ListItem value="財物類-其他非鐵金屬及其製品包括廢料及碎料瓷金及其製品含金屬或其化合物之灰末及渣滓不包括煉製鋼鐵所產生者">財物類-其他非鐵金屬及其製品包括廢料及碎料瓷金及其製品含金屬或其化合物之灰末及渣滓不包括煉製鋼鐵所產生者</asp:ListItem>
                                            <asp:ListItem value="財物類-其他特殊用途之機具及其零件">財物類-其他特殊用途之機具及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-其他通用機具">財物類-其他通用機具</asp:ListItem>
                                            <asp:ListItem value="財物類-其他運輸設備及其零件">財物類-其他運輸設備及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-其他電力設備及零件">財物類-其他電力設備及零件</asp:ListItem>
                                            <asp:ListItem value="財物類-其他預鑄金屬產品">財物類-其他預鑄金屬產品</asp:ListItem>
                                            <asp:ListItem value="財物類-其他製造商品">財物類-其他製造商品</asp:ListItem>
                                            <asp:ListItem value="財物類-其他橡膠產品">財物類-其他橡膠產品</asp:ListItem>
                                            <asp:ListItem value="財物類-其他礦石,礦物質">財物類-其他礦石,礦物質</asp:ListItem>
                                            <asp:ListItem value="財物類-林業及伐木產物">財物類-林業及伐木產物</asp:ListItem>
                                            <asp:ListItem value="財物類-武器 彈藥及其零件">財物類-武器 彈藥及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-武器, 彈藥及其零件">財物類-武器, 彈藥及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-肥皂, 清潔劑, 香水及盥洗用品">財物類-肥皂, 清潔劑, 香水及盥洗用品</asp:ListItem>
                                            <asp:ListItem value="財物類-金屬礦石">財物類-金屬礦石</asp:ListItem>
                                            <asp:ListItem value="財物類-非結構性陶瓷製品">財物類-非結構性陶瓷製品</asp:ListItem>
                                            <asp:ListItem value="財物類-建地及相關土地">財物類-建地及相關土地</asp:ListItem>
                                            <asp:ListItem value="財物類-建築預製結構">財物類-建築預製結構</asp:ListItem>
                                            <asp:ListItem value="財物類-玻璃及玻璃產品">財物類-玻璃及玻璃產品</asp:ListItem>
                                            <asp:ListItem value="財物類-耐火產品及結構性非耐火黏土產品">財物類-耐火產品及結構性非耐火黏土產品</asp:ListItem>
                                            <asp:ListItem value="財物類-計算機及其零件與配件">財物類-計算機及其零件與配件</asp:ListItem>
                                            <asp:ListItem value="財物類-食品 飲料及菸草處理之機具及其零件">財物類-食品 飲料及菸草處理之機具及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-食品, 飲料及菸草處理之機具及其零件">財物類-食品, 飲料及菸草處理之機具及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-娛樂及運動用船舶">財物類-娛樂及運動用船舶</asp:ListItem>
                                            <asp:ListItem value="財物類-家用電器及其零件">財物類-家用電器及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-氣體產生器 蒸餾設備 冷凍及空調設備 過濾機具">財物類-氣體產生器 蒸餾設備 冷凍及空調設備 過濾機具</asp:ListItem>
                                            <asp:ListItem value="財物類-氣體產生器; 蒸餾設備; 冷凍及空調設備; 過濾機具">財物類-氣體產生器; 蒸餾設備; 冷凍及空調設備; 過濾機具</asp:ListItem>
                                            <asp:ListItem value="財物類-烤箱, 熔爐燃燒器及其零件">財物類-烤箱, 熔爐燃燒器及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-紗及線 編織及簇織飾物">財物類-紗及線 編織及簇織飾物</asp:ListItem>
                                            <asp:ListItem value="財物類-紗及線, 編織及簇織飾物">財物類-紗及線, 編織及簇織飾物</asp:ListItem>
                                            <asp:ListItem value="財物類-紙漿,紙及紙產品;印刷品及相關的商品">財物類-紙漿,紙及紙產品;印刷品及相關的商品</asp:ListItem>
                                            <asp:ListItem value="財物類-紙漿紙及紙產品印刷品及相關的商品">財物類-紙漿紙及紙產品印刷品及相關的商品</asp:ListItem>
                                            <asp:ListItem value="財物類-航空器 太空船及其零件">財物類-航空器 太空船及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-航空器, 太空船及其零件">財物類-航空器, 太空船及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-除服裝以外之紡織品">財物類-除服裝以外之紡織品</asp:ListItem>
                                            <asp:ListItem value="財物類-做為測量、檢查、航行及其他目的用之儀器和裝置，除光學儀器; 工業程序控制設備; 上述各項之零件及附件">財物類-做為測量、檢查、航行及其他目的用之儀器和裝置，除光學儀器; 工業程序控制設備; 上述各項之零件及附件</asp:ListItem>
                                            <asp:ListItem value="財物類-做為測量檢查航行及其他目的用之儀器和裝置除光學儀器 工業程序控制設備 上述各項之零件及附件">財物類-做為測量檢查航行及其他目的用之儀器和裝置除光學儀器 工業程序控制設備 上述各項之零件及附件</asp:ListItem>
                                            <asp:ListItem value="財物類-基本化學產品">財物類-基本化學產品</asp:ListItem>
                                            <asp:ListItem value="財物類-基本的鐵與鋼">財物類-基本的鐵與鋼</asp:ListItem>
                                            <asp:ListItem value="財物類-基本貴金屬及包覆貴金屬之金屬">財物類-基本貴金屬及包覆貴金屬之金屬</asp:ListItem>
                                            <asp:ListItem value="財物類-瓶子洗滌器,包裝機器,秤重機具;噴灑機具">財物類-瓶子洗滌器,包裝機器,秤重機具;噴灑機具</asp:ListItem>
                                            <asp:ListItem value="財物類-船舶">財物類-船舶</asp:ListItem>
                                            <asp:ListItem value="財物類-傢具">財物類-傢具</asp:ListItem>
                                            <asp:ListItem value="財物類-結構金屬產品及其零件">財物類-結構金屬產品及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-絕緣電線 電纜及光纖電纜">財物類-絕緣電線 電纜及光纖電纜</asp:ListItem>
                                            <asp:ListItem value="財物類-絕緣電線, 電纜及光纖電纜">財物類-絕緣電線, 電纜及光纖電纜</asp:ListItem>
                                            <asp:ListItem value="財物類-軸承 齒輪 傳動裝置 驅動元件及其零件">財物類-軸承 齒輪 傳動裝置 驅動元件及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-軸承, 齒輪, 傳動裝置, 驅動元件及其零件">財物類-軸承, 齒輪, 傳動裝置, 驅動元件及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-塑料半製品">財物類-塑料半製品</asp:ListItem>
                                            <asp:ListItem value="財物類-塑料製包裝產品">財物類-塑料製包裝產品</asp:ListItem>
                                            <asp:ListItem value="財物類-煉焦爐產品；精煉石油產品；核燃料">財物類-煉焦爐產品；精煉石油產品；核燃料</asp:ListItem>
                                            <asp:ListItem value="財物類-煉焦爐產品精煉石油產品核燃料">財物類-煉焦爐產品精煉石油產品核燃料</asp:ListItem>
                                            <asp:ListItem value="財物類-農業或林業機具及其零件">財物類-農業或林業機具及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-運動商品">財物類-運動商品</asp:ListItem>
                                            <asp:ListItem value="財物類-遊戲及玩具">財物類-遊戲及玩具</asp:ListItem>
                                            <asp:ListItem value="財物類-鈾及釷礦石">財物類-鈾及釷礦石</asp:ListItem>
                                            <asp:ListItem value="財物類-電力 瓦斯 蒸氣及熱水">財物類-電力 瓦斯 蒸氣及熱水</asp:ListItem>
                                            <asp:ListItem value="財物類-電力, 瓦斯, 蒸氣及熱水">財物類-電力, 瓦斯, 蒸氣及熱水</asp:ListItem>
                                            <asp:ListItem value="財物類-電力傳輸、控制設備及其零件">財物類-電力傳輸、控制設備及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-電力傳輸控制設備及其零件">財物類-電力傳輸控制設備及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-電動機, 發電機, 變壓器及其附件">財物類-電動機, 發電機, 變壓器及其附件</asp:ListItem>
                                            <asp:ListItem value="財物類-滾壓 拉拔 摺疊製鋼鐵製品">財物類-滾壓 拉拔 摺疊製鋼鐵製品</asp:ListItem>
                                            <asp:ListItem value="財物類-滾壓, 拉拔, 摺疊製鋼鐵製品">財物類-滾壓, 拉拔, 摺疊製鋼鐵製品</asp:ListItem>
                                            <asp:ListItem value="財物類-漆類、清漆及相關產品；繪畫用色料；墨水">財物類-漆類、清漆及相關產品；繪畫用色料；墨水</asp:ListItem>
                                            <asp:ListItem value="財物類-漆類清漆及相關產品繪畫用色料墨水">財物類-漆類清漆及相關產品繪畫用色料墨水</asp:ListItem>
                                            <asp:ListItem value="財物類-蓄電池、一次性電池及其零件">財物類-蓄電池、一次性電池及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-蓄電池一次性電池及其零件">財物類-蓄電池一次性電池及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-蒸汽產生器(中央暖氣鍋爐除外)及其零件">財物類-蒸汽產生器(中央暖氣鍋爐除外)及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-蒸汽產生器中央暖氣鍋爐除外及其零件">財物類-蒸汽產生器中央暖氣鍋爐除外及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-製造紡織品, 服裝及皮革產品之機具及其零件">財物類-製造紡織品, 服裝及皮革產品之機具及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-銅, 鎳, 鋁, 氧化鋁, 鉛,鋅及錫或其合金之半製品">財物類-銅, 鎳, 鋁, 氧化鋁, 鉛,鋅及錫或其合金之半製品</asp:ListItem>
                                            <asp:ListItem value="財物類-樂器">財物類-樂器</asp:ListItem>
                                            <asp:ListItem value="財物類-穀粉 澱粉及澱粉製品 其他食品">財物類-穀粉 澱粉及澱粉製品 其他食品</asp:ListItem>
                                            <asp:ListItem value="財物類-編織或鉤針織品">財物類-編織或鉤針織品</asp:ListItem>
                                            <asp:ListItem value="財物類-機動車 拖車 半拖車 車輛機件">財物類-機動車 拖車 半拖車 車輛機件</asp:ListItem>
                                            <asp:ListItem value="財物類-機動車, 拖車, 半拖車; 車輛機件">財物類-機動車, 拖車, 半拖車; 車輛機件</asp:ListItem>
                                            <asp:ListItem value="財物類-燈絲電燈泡或放電式燈泡；弧光燈；照明設備；上述各項之零件">財物類-燈絲電燈泡或放電式燈泡；弧光燈；照明設備；上述各項之零件</asp:ListItem>
                                            <asp:ListItem value="財物類-辦公室及會計機器，其零件及附件">財物類-辦公室及會計機器，其零件及附件</asp:ListItem>
                                            <asp:ListItem value="財物類-辦公室及會計機器其零件及附件">財物類-辦公室及會計機器其零件及附件</asp:ListItem>
                                            <asp:ListItem value="財物類-幫浦 壓縮機 液壓與氣動引擎 閥門及其零件">財物類-幫浦 壓縮機 液壓與氣動引擎 閥門及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-幫浦, 壓縮機, 液壓與氣動引擎, 閥門及其零件">財物類-幫浦, 壓縮機, 液壓與氣動引擎, 閥門及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-醫療 外科及矯形設備">財物類-醫療 外科及矯形設備</asp:ListItem>
                                            <asp:ListItem value="財物類-醫療, 外科及矯形設備">財物類-醫療, 外科及矯形設備</asp:ListItem>
                                            <asp:ListItem value="財物類-醫藥產品">財物類-醫藥產品</asp:ListItem>
                                            <asp:ListItem value="財物類-鐘錶及其零件">財物類-鐘錶及其零件</asp:ListItem>
                                            <asp:ListItem value="財物類-鐵, 鋼或鋁製大桶、水箱及容器">財物類-鐵, 鋼或鋁製大桶、水箱及容器</asp:ListItem>
                                            <asp:ListItem value="財物類-鐵路及電車軌道之機車及其零件">財物類-鐵路及電車軌道之機車及其零件</asp:ListItem>
                                            <asp:ListItem value="勞務類">勞務類</asp:ListItem>
                                            <asp:ListItem value="勞務類-人力派遣服務">勞務類-人力派遣服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-土木工程施工服務">勞務類-土木工程施工服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-工程服務">勞務類-工程服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-工程相關之科學及技術諮詢服務">勞務類-工程相關之科學及技術諮詢服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-不配備操作員之租賃服務">勞務類-不配備操作員之租賃服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-不動產服務">勞務類-不動產服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-支援及輔助運輸服務">勞務類-支援及輔助運輸服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-水上運輸服務">勞務類-水上運輸服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-市場研究及民意調查服務">勞務類-市場研究及民意調查服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-污水及垃圾處理、公共衛生及其他環保服務">勞務類-污水及垃圾處理、公共衛生及其他環保服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-污水及垃圾處理公共衛生及其他環保服務">勞務類-污水及垃圾處理公共衛生及其他環保服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-技術測試及分析服務">勞務類-技術測試及分析服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-其他服務">勞務類-其他服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-其他商業服務">勞務類-其他商業服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-法律服務">勞務類-法律服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-空運服務">勞務類-空運服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-保險及退休基金輔助服務">勞務類-保險及退休基金輔助服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-建築服務">勞務類-建築服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-建築施工服務">勞務類-建築施工服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-研發服務">勞務類-研發服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-娛樂,文化,體育服務">勞務類-娛樂,文化,體育服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-娛樂文化體育服務">勞務類-娛樂文化體育服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-針對整個社區之服務">勞務類-針對整個社區之服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-除機動車輛及機器腳踏車以外之經紀商及批發貿易服務">勞務類-除機動車輛及機器腳踏車以外之經紀商及批發貿易服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-健康及社會服務">勞務類-健康及社會服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-教育服務">勞務類-教育服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-都市計劃及景觀建築服務">勞務類-都市計劃及景觀建築服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-陸地運輸服務">勞務類-陸地運輸服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-會計、審計及簿記服務">勞務類-會計、審計及簿記服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-會員組織服務">勞務類-會員組織服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-農業 礦業及製造業服務">勞務類-農業 礦業及製造業服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-農業, 礦業及製造業服務">勞務類-農業, 礦業及製造業服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-電信服務">勞務類-電信服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-電信相關服務">勞務類-電信相關服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-電腦及相關服務">勞務類-電腦及相關服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-零售業服務；個人及家用品維修服務">勞務類-零售業服務；個人及家用品維修服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-零售業服務個人及家用品維修服務">勞務類-零售業服務個人及家用品維修服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-團體;組織;法人提供的服務">勞務類-團體;組織;法人提供的服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-團體組織法人提供的服務">勞務類-團體組織法人提供的服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-綜合工程服務">勞務類-綜合工程服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-廣告服務">勞務類-廣告服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-廣播及有線電視服務">勞務類-廣播及有線電視服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-樓宇清潔服務">勞務類-樓宇清潔服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-調查及保安服務">勞務類-調查及保安服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-機動車輛及機器腳踏車銷售、維修服務">勞務類-機動車輛及機器腳踏車銷售、維修服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-機動車輛及機器腳踏車銷售維修服務">勞務類-機動車輛及機器腳踏車銷售維修服務</asp:ListItem>
                                            <asp:ListItem value="勞務類-餐點服務">勞務類-餐點服務</asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                            </table>
             
                    <div class="text-center">
                        <asp:Button ID="btnQuery" CssClass="btn-success btnw2" runat="server" Text="查詢" CommandArgument="<%=data_year%>" OnClick="btnQuery_Click"/>
                        <asp:Button ID="btnReset" CssClass="btn-success btnw2" runat="server" Text="清除" OnClick="btnReset_Click"/>
                    </div>
                </asp:Panel>
                     <div class="text-center">
                         <br />
                        <asp:Button id="btnPrint" Cssclass="btn-success btnw2" Text="將結果轉成excel"  Onclick="btnPrint_Click" runat="server" Visible="False" Width="150px"/>

                        <br />
                    </div>
                    <asp:GridView ID="GV_A" CellPadding="0" DataKeyNames="OVC_KEY" CssClass="table data-table table-striped border-top text-center data-table" AutoGenerateColumns="false" runat="server" OnPreRender="GV_A_PreRender" >



                         <Columns>
                                    <asp:TemplateField HeaderText="項次">
                                    <ItemTemplate>
                                       <%#Container.DataItemIndex + 1%>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="False"  />
                                    <ItemStyle  HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="採購案號" DataField="OVC_KEY" /> 
                                    <asp:BoundField HeaderText="標的名稱" DataField="OVC_W" />
                                    <asp:BoundField HeaderText="決標日期" DataField="OVC_AQ"   DataFormatString="{0:d}"/>
                                    <asp:BoundField HeaderText="廠商名稱" DataField="OVC_A" />
                                    <asp:BoundField HeaderText="招標方式" DataField="OVC_AW" />
                                    <asp:BoundField HeaderText="決標方式" DataField="OVC_BC" />
                                    <asp:BoundField HeaderText="標的分類" DataField="OVC_AT" />
                                    <asp:TemplateField HeaderText="執行" ItemStyle-CssClass="text-center">
										<ItemTemplate>
                                            <%if (Session["data_year"].ToString().Equals("99"))
                                                      {%>
                                                <a href="javascript:var win=window.open('CIMS_C12_2.aspx?OVC_KEY=<%# Eval("OVC_KEY")%>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=1000,height=700,left=0,top=0');">
                                                 查詢購案明細</a><%} %>		
                                            <%else
                                                {%>
                                            <a href="javascript:var win=window.open('CIMS_C12_3.aspx?OVC_KEY=<%# Eval("OVC_KEY")%>',null,'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=no,minimizebutton=no,copyhistory=no,width=1000,height=700,left=0,top=0');">
                                                 查詢購案明細</a><%} %>	
										</ItemTemplate>
									</asp:TemplateField>

                            <%--                            <asp:TemplateField HeaderText="附檔名稱">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDownload" Text='<%# Eval("OVC_FILE_NAME")%>' CommandName="downloadfile" runat="server"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="指令">
                                <ItemTemplate>
                                    <asp:Button CssClass="btn-info btnw2" CommandName="Del" runat="server" Text="刪除" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
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
