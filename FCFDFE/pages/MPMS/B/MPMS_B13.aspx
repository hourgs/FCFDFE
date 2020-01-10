<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_B13.aspx.cs" Inherits="FCFDFE.pages.MPMS.B.MPMS_B13" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <!--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>-->
   <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
       });
    </script>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                </header>
                <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                    <ContentTemplate>
                    <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div id="tabs" class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <!--頁籤－開始-->
                                <header class="panel-heading">
                                    <ul class="nav nav-tabs" ID="myTabs">
                                        <!--各頁籤-->
                                        <li ID="pageOne" class="active" runat="server"><!--起始選取頁籤，只有一個-->
                                            <a data-toggle="tab" href="#first">購案物資申請書編製</a>
                                        </li>
                                        <li ID="pageSecond" class="" runat="server"><!--尚未選取頁籤-->
                                            <a data-toggle="tab" href="#second">計畫編製計畫清單畫面</a>
                                        </li>
                                        <li ID="pageThird" class="" runat="server"><!--尚未選取頁籤-->
                                            <a data-toggle="tab" href="#third">複數決標案--計畫清單項目分組作業</a>
                                        </li>
                                    </ul>
                                </header>
                                <div class="panel-body tab-body">
                                    <div class="tab-content">
                                        <!--各標籤之頁面-->
                                        <!--購案物資申請書編制 PAGE1 -->
                                        <div id="first"  class="tab-pane active">
                                            <!--起始選取頁面，只有一個-->
                                             <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                                  <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnGOOD_APPLICATION_PRINT" />
                                                    <asp:PostBackTrigger ControlID="btnOLD_GOOD_APPLICATION_PRINT" />
                                                </Triggers>
                                            <ContentTemplate>
                                            <table class="table table-bordered" style="text-align: left; margin: 0px;">
                                                <tr>
                                                    <td rowspan="2">
                                                        <asp:TextBox ID="txtOVC_KIND_APPLY" CssClass="tb tb-m" runat="server">國防部政務辦公室</asp:TextBox>
                                                        <asp:Label ID="lblStubIn" CssClass="control-label" runat="server">  內購案物資申請書存根 </asp:Label>
                                                        <asp:Label ID="lblStubOut" CssClass="control-label" runat="server">  外購案物資申請書存根 </asp:Label>
                                                        <asp:CheckBox ID="chkChangeApply" CssClass="control-label" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server" text="同時變更申辦單位"/><br><br>
                                                        <asp:Label CssClass="control-label text-red" runat="server">聯勤請更改為[國防部聯合後勤司令部](用於第二聯)</asp:Label>
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:Label CssClass="control-label" runat="server">中華民國</asp:Label>
                                                        <asp:Label ID ="lblOVC_PUR_DAPPROVE" CssClass="control-label" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="text-right"> 
                                                        <asp:Label ID="lblOVC_PUR_APPROVE" CssClass="control-label" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="table table-bordered" style="text-align: left; margin: 0px;">
                                                <tr>
                                                    <td style="width: 25%">
                                                        <asp:Label CssClass="control-label" runat="server">申購單位</asp:Label>
                                                    </td>
                                                    <td style="width: 25%">
                                                        <asp:TextBox ID="txtOVC_PUR_NSECTION" CssClass="tb tb-m" AutoGenerateColumns="false" runat="server">國防部政務辦公室</asp:TextBox>
                                                    </td>
                                                    <td style="width: 25%">
                                                        <asp:Label CssClass="control-label" runat="server">申購日期及文號</asp:Label>
                                                    </td>
                                                    <td style="width: 25%" class="text-right">
                                                        <asp:Label CssClass="control-label" runat="server">中華民國</asp:Label>
                                                        <asp:Label ID="lblOVC_DPROPOSE" CssClass="control-label" runat="server"></asp:Label><br>
                                                        <asp:Label ID="lblOVC_PROPOSE" CssClass="control-label" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="table table-bordered" style="text-align: center; margin: 0px;">
                                                <tr>
                                                    <td rowspan="4" style="width: 10px;">
                                                        <asp:Label CssClass="control-label " runat="server">物品種類及數量</asp:Label></td>
                                                    <td>
                                                        <asp:Label CssClass="control-label " runat="server">名稱</asp:Label></td>
                                                    <td>
                                                        <asp:Label CssClass="control-label" runat="server">單位</asp:Label></td>
                                                    <td>
                                                        <asp:Label CssClass="control-label" runat="server">總價</asp:Label></td>
                                                    <td>
                                                        <asp:Label CssClass="control-label" runat="server">數量</asp:Label></td>
                                                    <td class="no-bordered">
                                                        <asp:Label CssClass="control-label" runat="server">預算來源</asp:Label></td>
                                                    <td class="no-bordered">
                                                        <asp:Button ID="btnModify_BUDGET" CssClass="btn-success btnw4" runat="server" OnClick="btnModify_BUDGET_Click" Text="預算編輯" /><!--綠色--></td>
                                                </tr>
                                                <tr>
                                                    <td rowspan="2">
                                                        <asp:Label CssClass="control-label" runat="server">購案名稱</asp:Label></td>
                                                    <td>
                                                        <asp:Label CssClass="control-label" runat="server">中文名稱</asp:Label></td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtOVC_PUR_IPURCH" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                                    <td>
                                                        <asp:Label CssClass="control-label " runat="server">款項</asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblOVC_ISOURCE" CssClass="control-label " runat="server"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label CssClass="control-label " runat="server">英文名稱</asp:Label></td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtOVC_PUR_IPURCH_ENG" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                                    <td>
                                                        <asp:Label CssClass="control-label " runat="server">科目</asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblOVC_PJNAME" CssClass="control-label " runat="server"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="drp" CssClass="tb tb-m" runat="server">
                                                            <asp:ListItem>貨款總價</asp:ListItem>
                                                            <asp:ListItem>預估貨款總價</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblONB_MONEY_NT_CHI" CssClass="control-label " runat="server">新台幣零元(含稅)</asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblONB_MONEY_NT_NUM" CssClass="control-label " runat="server">0.00</asp:Label></td>
                                                    <td>
                                                        <asp:Label CssClass="control-label" runat="server">奉准日期及文號</asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblOVC_PUR_DAPPR_PLAN_WITH" CssClass="control-label" runat="server"></asp:Label></td>
                                                </tr>
                                            </table>
                                            <table class="table table-bordered" style="text-align: center; margin: 0px">
                                                <tr>
                                                    <td colspan="20" class="screentone-gray">
                                                        <asp:Label ID="Label1" CssClass="control-label" runat="server"><b>工程會必要資料(必須要輸入)</b></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td rowspan="3" colspan="4">
                                                        <asp:Label CssClass="control-label text-star" runat="server">標的分類</asp:Label>
                                                    </td>
                                                    <td colspan="16" class="text-left">
                                                        <asp:RadioButtonList ID="rdoOVC_LAB" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="rdoOVC_LAB_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                                        <asp:ListItem Value="1">勞務採購</asp:ListItem>
                                                        <asp:ListItem Value="2">財務採購(買受、訂製)</asp:ListItem>
                                                        <asp:ListItem Value="3">財務採購(租實)</asp:ListItem>
                                                        <asp:ListItem Value="4">財務採購(租購)</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="16">
                                                        <asp:DropDownList ID="drpStandardType" CssClass="tb tb-full" OnSelectedIndexChanged="drpStandardType_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered">
                                                    <td colspan="4" class="">
                                                        <asp:Label CssClass="control-label text-star" runat="server">標的分類代碼：</asp:Label>
                                                    </td>
                                                    <td colspan="4" class="">
                                                        <asp:TextBox ID="txtOVC_TARGET_KIND" CssClass="tb tb-full" AutoPostBack="true" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td colspan="12"></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label text-star" runat="server">履約地點</asp:Label>
                                                    </td>
                                                    <td colspan="8">
                                                        <asp:DropDownList ID="drpOVC_RECEIVE_PLACE" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label text-star" runat="server">履約期限</asp:Label>
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:TextBox ID="txtOVC_SHIP_TIMES" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Label CssClass="control-label" runat="server">廠商資格摘要</asp:Label>
                                                    </td>
                                                    <td colspan="16">
                                                        <asp:TextBox ID="txtOVC_VENDOR_DESC" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                </table>
                                            <div id="OutPart" runat="server">
                                                <table class="table table-bordered text-center">
                                                    <tr>
                                                        <td colspan="4">
                                                            <asp:Label CssClass="control-label" runat="server">用途</asp:Label>
                                                        </td>
                                                        <td colspan="12">
                                                            <asp:TextBox ID="txtUse" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:Button ID="btnUseEditing" CssClass="btn-success btnw4" OnClick="btnUseEditing_Click" runat="server" Text="用途編輯" /><!--綠色-->
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <asp:Label CssClass="control-label" runat="server">外購理由</asp:Label>
                                                        </td>
                                                        <td colspan="12">
                                                            <asp:TextBox ID="txtOutPur" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:Button ID="btnOutPurEditing" CssClass="btn-success" runat="server" OnClick="btnOutPurEditing_Click" Text="外購理由編輯" /><!--綠色-->
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                                <table class="table table-bordered text-center">
                                                <tr>
                                                    <td rowspan="3" colspan="4">
                                                        <asp:Label CssClass="control-label" runat="server">相關稅賦</asp:Label>
                                                    </td>
                                                    <td colspan="6">
                                                        <asp:Label CssClass="control-label text-red" runat="server">關稅及進口營業稅</asp:Label>
                                                        <asp:RadioButtonList ID="rdoOVC_PUR_FEE_OK" CssClass="radioButton rb-complex" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                                        <asp:ListItem Value="Y">免</asp:ListItem>
                                                        <asp:ListItem Value="N">不能免</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                        <asp:HyperLink ID="lnkTaxes1" runat="server" NavigateUrl="~/pages/MPMS/B/taxe1.html">軍用物品進口免稅辦法</asp:HyperLink>
                                                    </td>
                                                    <td colspan="5">
                                                        <asp:Label CssClass="control-label text-red" runat="server">營業稅</asp:Label>
                                                        <asp:RadioButtonList ID="rdoOVC_PUR_TAX_OK" CssClass="radioButton rb-complex" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                                        <asp:ListItem Value="Y">免</asp:ListItem>
                                                        <asp:ListItem Value="N">不能免</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                        <asp:HyperLink ID="lnkTaxes2" runat="server" NavigateUrl="~/pages/MPMS/B/taxe3.html">軍用貨品免繳營業稅作業規定</asp:HyperLink>
                                                    </td>
                                                    <td colspan="5">
                                                        <asp:Label CssClass="control-label text-red" runat="server">貨物稅</asp:Label>
                                                        <asp:RadioButtonList ID="rdoOVC_PUR_GOOD_OK" CssClass="radioButton rb-complex" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                                        <asp:ListItem Value="Y">免</asp:ListItem>
                                                        <asp:ListItem Value="N">不能免</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                         <asp:HyperLink ID="lnkTaxes3" runat="server" NavigateUrl="~/pages/MPMS/B/taxe2.html">軍用貨品貨物稅免稅辦法</asp:HyperLink>
                                                    </td>
                                                </tr>
                                                <tr class="no-bordered">
                                                    <td colspan="1">
                                                        <asp:Label CssClass="control-label" runat="server">其它：</asp:Label>
                                                    </td>
                                                    <td colspan="11">
                                                        <asp:TextBox ID="txtOTHER" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td colspan="8"></td>
                                                </tr>
                                            </table>
                                            <table class="table table-bordered" style="text-align: center;margin:0px;">
                                                <tr>
                                                <td rowspan="3" style="width: 50px;">
                                                    <asp:Label CssClass="control-label" runat="server">請求事項 </asp:Label>
                                                </td>
                                                    <td class="text-left">
                                                        <asp:Label CssClass="control-label text-red" runat="server">本案為複數決標</asp:Label>
                                                        <asp:DropDownList ID="drpIS_PLURAL_BASIS" CssClass="tb" runat="server"></asp:DropDownList>
                                                        <asp:Label CssClass="control-label text-red" runat="server">本案為開放式契約</asp:Label>
                                                        <asp:DropDownList ID="drpIS_OPEN_CONTRACT" CssClass="tb" runat="server"></asp:DropDownList>
                                                        <asp:Label CssClass="control-label text-red" runat="server">本案為並列得標廠商</asp:Label>
                                                        <asp:DropDownList ID="drpIS_JUXTAPOSED_MANUFACTURER" CssClass="tb" runat="server"></asp:DropDownList><br />
                                                        <asp:Label CssClass="control-label text-blue" runat="server">(如為複數決標請記得要做分組作業及備考編輯的預算分配加入備考中)</asp:Label>
                                                    </td>
                                                    <td rowspan="3" class="td-vertical">
                                                        <asp:Button ID="btnModify" CssClass="btn-success" OnClick="btnModify_Click" runat="server" Text="請求事項編輯" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="text-left">
                                                        <asp:Label CssClass="control-label text-red" runat="server">計劃性質：</asp:Label>
                                                        <asp:DropDownList ID="drpOVC_PLAN_PURCH" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtOVC_MEMO_REQUEST" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label CssClass="control-label" runat="server">附件</asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Button ID="btnFileUpload" CssClass="btn-success" runat="server" OnClick="btnFileUpload_Click" Text="附件及檔案上傳" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label CssClass="control-label" runat="server">備考</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtOVC_MEMO" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnModify_MEMO" CssClass="btn-success btnw4" OnClick="btnModify_MEMO_Click" runat="server" Text="備考編輯" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table class="table table-bordered" style="text-align: center;margin:0px;">
                                                <tr>
                                                    <td class="text-left" colspan="4">
                                                        <asp:Label ID="lblTOUNIT" CssClass="control-label" runat="server">謹呈</asp:Label>　
                                                        <asp:TextBox ID="txtOVC_SUPERIOR_UNIT" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td class="text-right" colspan="4">
                                                        <asp:Label CssClass="control-label" runat="server">主官</asp:Label>　
                                                        <asp:TextBox ID="txtOVC_SECTION_CHIEF" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <asp:Label CssClass="control-label" runat="server">審查意見</asp:Label>　
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label CssClass="control-label" runat="server">承辦人</asp:Label>　
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label CssClass="control-label" runat="server">會辦單位</asp:Label>　
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label CssClass="control-label" runat="server">會辦單位</asp:Label>　
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label CssClass="control-label" runat="server">承辦單位</asp:Label>　
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label CssClass="control-label" runat="server">核稿人</asp:Label>　
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label CssClass="control-label" runat="server"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label CssClass="control-label" runat="server"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblOVC_DOING_UNIT" CssClass="control-label" runat="server"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label CssClass="control-label" runat="server">核辦人</asp:Label>　
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td rowspan="2" colspan="1">
                                                        <asp:Label CssClass="control-label" runat="server">承辦人</asp:Label>　
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label CssClass="control-label" runat="server">姓名：</asp:Label>
                                                        <asp:TextBox ID="txtOVC_USER" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td rowspan="2" colspan="1">
                                                        <asp:Label CssClass="control-label" runat="server">聯絡電話</asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label CssClass="control-label" runat="server">自動：</asp:Label>
                                                        <asp:TextBox ID="txtOVC_PUR_IUSER_PHONE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label CssClass="control-label" runat="server">軍線1　</asp:Label>
                                                        <asp:TextBox ID="txtOVC_PUR_IUSER_PHONE_EXT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label CssClass="control-label" runat="server">階級：</asp:Label>
                                                        <asp:TextBox ID="txtOVC_USER_TITLE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label CssClass="control-label" runat="server">手機：</asp:Label>
                                                        <asp:TextBox ID="txtOVC_USER_CELLPHONE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label CssClass="control-label" runat="server">軍線2　</asp:Label>
                                                        <asp:TextBox ID="txtOVC_PUR_IUSER_PHONE_EXT1" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="text-center" style="letter-spacing: 15px; margin-top:30px;">
                                                <asp:Button ID="btnSave_New" CssClass="btn-success btnw2" runat="server" onclick="btnSave_New_Click" Text="儲存" />
                                                <asp:Button ID="btnSave_Modify" CssClass="btn-warning btnw4" runat="server" onclick="btnSave_Modify_Click" Text="儲存修改" />
                                                <asp:Button ID="btnReturn" CssClass="btn-success" runat="server" onclick="btnReturn_Click" Text="處理其他購案" />
                                                <asp:Button ID="btnGOOD_APPLICATION_PRINT" CssClass="btn-success" CommandName="PrintSupPDF" OnCommand="btnPrint_Command" Visible ="false" runat="server" Text="物資申請書預覽列印" /><!--綠色-->
                                                <asp:Button ID="btnOLD_GOOD_APPLICATION_PRINT" CssClass="btn-success" runat="server" CommandName="PrintNewSupPDF" Visible ="false" OnCommand="btnPrint_Command" Text="新物資申請書預覽列印" /><!--綠色-->
                                            </div>
                                                 </ContentTemplate>
                                        </asp:UpdatePanel>
                                        </div>
                                        <%--計畫編製計畫清單畫面 PAGE2--%>
                                        <div id="second" class="tab-pane"><!--尚未選取頁面-->
                                             <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                                                 <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnSave_PLANLIST_IN" />
                                                    <asp:PostBackTrigger ControlID="btnSave_PLANLIST" />
                                                    
                                                </Triggers>
                                            <ContentTemplate>
                                            <%--內購計畫清單--%>
                                            <asp:Panel ID="divSecContentIN" Visible="false" runat="server">
                                                <div class="text-center">
                                                    <asp:Label CssClass="control-label" runat="server" Text="內購案採購計畫清單編制"></asp:Label>
                                                </div>
                                                <table class="table table-bordered text-center">
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <asp:Label CssClass="control-label" runat="server">(1)軍品類別</asp:Label>
                                                        </td>
                                                        <td class="text-left" style="width: 30%">
                                                            <asp:DropDownList ID="drpOVC_PUR_NPURCH_IN" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                                        </td>
                                                        <td style="width: 20%">
                                                            <asp:Label CssClass="control-label" runat="server">(6)購案編號</asp:Label>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblOVC_PURCH_IN" CssClass="control-label" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(2)預算來源</asp:Label>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblONB_PUR_BUDGET_FORM_IN" CssClass="control-label" runat="server"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(7)接收單位</asp:Label>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:TextBox ID="txtOVC_RECEIVE_NSECTION_IN" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(3)預算奉准文號日期</asp:Label>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblOVC_PUR_DAPPR_PLAN_IN" CssClass="control-label" runat="server"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(8)交貨時間</asp:Label>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:TextBox ID="txtOVC_SHIP_TIMES_IN" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(4)計劃申購單位</asp:Label>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblOVC_PUR_NSECTION_IN" CssClass="control-label" runat="server"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(9)交貨地點</asp:Label>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:TextBox ID="txtOVC_RECEIVE_PLACE_IN" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(5)採購單位</asp:Label>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: left;">
                                                                <asp:DropDownList ID="drpOVC_AGNT_IN_IN" CssClass="tb tb-l" OnSelectedIndexChanged="drpOVC_AGNT_IN_IN_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                                <asp:HiddenField ID="txtOVC_DEPT_CDE" runat="server"/>
                                                                <asp:Button ID="btnQuery_IN" OnClick="btnQuery_IN_Click" OnClientClick="OpenWindow('txtOVC_DEPT_CDE','txtOVC_AGNT_IN_SHOW_IN')" CssClass="btn-success btnw4" runat="server" Text="單位查詢" />
                                                            </div>
                                                            <br />
                                                            <br />
                                                            <div style="text-align: left;">
                                                                <asp:TextBox ID="txtOVC_AGNT_IN_SHOW_IN" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(10)檢驗方法</asp:Label>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:TextBox ID="txtOVC_POI_IINSPECT" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="text-center">
                                                    <asp:Button ID="btnSave_plan_IN" CssClass="btn-warning btnw2" OnClick="btnSave_plan_IN_Click" runat="server" Text="存檔" />
                                                </div>
                                                <header class="title">
                                                    <!--標題-->
                                                    計劃清單編製
                                                </header>
                                                <table class="table table-bordered text-center">
                                                    <tr>
                                                        <td colspan="11" class="text-left">
                                                            <asp:Label CssClass="control-label" runat="server">(11)項次　　</asp:Label>
                                                            <asp:Label ID ="lblONB_POI_ICOUNT_IN" CssClass="control-label" runat="server">1</asp:Label>
                                                           
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td rowspan="5" style="width: 25px;">
                                                            <asp:Label CssClass="control-label" runat="server">(12)品名料號及規格</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">中文品名</asp:Label></td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:TextBox ID="txtOVC_POI_NSTUFF_CHN_IN" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:Label CssClass="control-label" runat="server">英文品名</asp:Label></td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:TextBox ID="txtOVC_POI_NSTUFF_ENG_IN" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">料號種類</asp:Label></td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:DropDownList ID="drpNSN_KIND_IN" CssClass="tb tb-s" runat="server">
                                                                <asp:ListItem>請選擇</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">料號</asp:Label></td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:TextBox ID="txtNSN_IN" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">廠牌</asp:Label></td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:TextBox ID="txtOVC_BRAND_IN" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                            <asp:CheckBox ID="chkOVC_SAME_QUALITY_BRAND_IN" runat="server" CssClass="radioButton" />(或同等品)
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">型號</asp:Label>
                                                        </td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:TextBox ID="txtOVC_MODEL_IN" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                            <asp:CheckBox ID="chkOVC_SAME_QUALITY_MODEL_IN" CssClass="radioButton" runat="server" />(或同等品)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">件號</asp:Label></td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:TextBox ID="txtOVC_POI_IREF_IN" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                            <asp:CheckBox ID="chkOVC_SAME_QUALITY_POI_IREF_IN" CssClass="rb-xs radioButton" runat="server" />(或同等品)
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">行政院財產分類編</asp:Label>
                                                        </td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:TextBox ID="txtOVC_FCODE_IN" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                            <asp:HiddenField ID="txtName2" runat="server" />
                                                            <asp:Button ID="btnQuery_NUM_IN" CssClass="btn-success btnw4" OnClick="btnQuery_NUM_IN_Click" OnClientClick="OpenPhraseWindow('txtOVC_FCODE_IN','txtName2')" runat="server" Text="編號查詢" /><!--綠色-->
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">規格或性能</asp:Label>
                                                        </td>
                                                        <td colspan="9">
                                                            <asp:TextBox ID="txtOVC_INSPECT_IN" TextMode="MultiLine" Rows="5" CssClass="text-left textarea tb-full" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label CssClass="control-label" runat="server">(13)單位</asp:Label>
                                                        </td>
                                                        <td colspan="9" class="text-left">
                                                            <asp:DropDownList ID="drpOVC_POI_IUNIT_IN" CssClass="tb tb-s" runat="server">
                                                                <asp:ListItem>EA個</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label CssClass="control-label" runat="server">(14)數量</asp:Label>
                                                        </td>
                                                        <td colspan="9" class="text-left">
                                                            <asp:TextBox ID="txtONB_POI_QORDER_PLAN_IN" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label CssClass="control-label" runat="server">(15)單價</asp:Label>
                                                        </td>
                                                        <td colspan="9" class="text-left">
                                                            <asp:TextBox ID="txtONB_POI_MPRICE_PLAN_IN" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" rowspan="3">
                                                            <asp:Label CssClass="control-label" runat="server">(16)備考</asp:Label>
                                                        </td>
                                                        <td colspan="2" rowspan="3">
                                                            <asp:Label CssClass="control-label" runat="server">初次購買</asp:Label>
                                                        </td>
                                                        <td rowspan="3">
                                                            <asp:DropDownList ID="drpOVC_FIRST_BUY_IN" CssClass="tb tb-s" runat="server">
                                                                <asp:ListItem Value ="Y">Y:是</asp:ListItem>
                                                                <asp:ListItem Value ="N">N:否</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td rowspan="2">
                                                            <asp:RadioButton ID="chk_OVC_NUM_IN" GroupName="GroupBEF" CssClass="radioButton" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">案號</asp:Label>
                                                        </td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:TextBox ID="txtOVC_POI_IPURCH_BEF_IN" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">以往購價</asp:Label>
                                                        </td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:Label CssClass="control-label" runat="server">數量</asp:Label>
                                                            <asp:TextBox ID="txtONB_POI_QORDER_BEF_IN" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server">幣別</asp:Label>
                                                            <asp:DropDownList ID="drpOVC_CURR_MPRICE_BEF_IN" CssClass="tb tb-s" runat="server">
                                                                <asp:ListItem>請選擇</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:Label CssClass="control-label" runat="server">金額</asp:Label>
                                                            <asp:TextBox ID="txtONB_POI_MPRICE_BEF_IN" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="chk_OVC_SAME_IN" GroupName="GroupBEF" CssClass="radioButton" runat="server" />
                                                        </td>
                                                        <td colspan="5" class="text-left">
                                                            <asp:Label CssClass="control-label" runat="server">詳如商情資料</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label CssClass="control-label" runat="server">其它說明</asp:Label>
                                                        </td>
                                                        <td colspan="9" class="text-left">
                                                            <asp:TextBox ID="txtOVC_POI_NDESC_IN" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="text-center">
                                                    <asp:Button ID="btnSave_DETIAL_IN" OnClick="btnSave_DETIAL_IN_Click" CssClass="btn-warning" runat="server" Text="採購明細存檔" /><!--黃色-->
                                                    <asp:Button ID="btnSave_PLANLIST_IN" CssClass="btn-success" CommandName="PrintListInPDF" OnCommand="btnPrint_Command" runat="server" Text="計劃清單預覽列印" /><!--黃色-->
                                                </div>
                                                <div class="subtitle">採購明細結果顯示</div>
                                                <asp:GridView ID="GridView1_IN" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" ShowFooter="true" OnPreRender="GridView1_IN_PreRender" runat="server">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="項次" DataField="ONB_POI_ICOUNT" />
                                                        <asp:BoundField HeaderText="料號" DataField="NSN" />
                                                        <asp:BoundField HeaderText="中文品名" DataField="OVC_POI_NSTUFF_CHN" />
                                                        <asp:BoundField HeaderText="數量" DataField="ONB_POI_QORDER_PLAN" />
                                                        <asp:BoundField HeaderText="單價" DataField="ONB_POI_MPRICE_PLAN" />
                                                        <asp:TemplateField HeaderText="功能">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btn_change_IN" OnClick="btn_change_IN_Click" CssClass="btn-success btnw2" Text="異動" CommandName="按鈕屬性" runat="server" />
                                                                <asp:Button ID="btnDel_IN" OnClick="btnDel_IN_Click" CssClass="btn-danger btnw2" Text="刪除" CommandName="按鈕屬性" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <table class="table table-bordered text-center">
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <asp:Label CssClass="control-label" runat="server">(17)備註</asp:Label>
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <asp:Button ID="btnSave_OVC_MEMO_IN" OnClick="btnSave_OVC_MEMO_IN_Click" CssClass="btn-success btnw6" runat="server" Text="備註編輯" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <asp:Label CssClass="control-label" runat="server">(18)</asp:Label>
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <asp:Label ID="Label2" CssClass="control-label" runat="server">貨款總價：</asp:Label>
                                                            <asp:Label ID="lblOVC_GOOD_TOTAL_IN" CssClass="control-label" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <asp:Label CssClass="control-label" runat="server">(19)</asp:Label>
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <asp:Button ID="btnSave_WITH_IN" CssClass="btn-success " runat="server" OnClick="btnSave_WITH_IN_Click" Text="隨案檢附草約編輯" />
                                                            <asp:Label ID="lbl_WITH_IN" CssClass="control-label" runat="server">隨案檢附草約 份</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <asp:Label CssClass="control-label" runat="server">(20)附件</asp:Label>
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <asp:Button ID="btnFileUpload_Page2_IN" CssClass="btn-success" runat="server" OnClick="btnFileUpload_Page2_Click" Text="附件及檔案上傳" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="divSecContentOUT" Visible="false" runat="server">
                                                <%--外購計畫清單--%>
                                                <div class="text-center">
                                                    <asp:Label CssClass="control-label" runat="server" Text="外購案採購計畫清單編制"></asp:Label>
                                                </div>
                                                <table class="table table-bordered text-center">
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <asp:Label CssClass="control-label" runat="server">(1)軍品類別</asp:Label>
                                                        </td>
                                                        <td class="text-left" style="width: 30%">
                                                            <asp:DropDownList ID="drpOVC_PUR_NPURCH" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                                        </td>
                                                        <td style="width: 20%">
                                                            <asp:Label CssClass="control-label" runat="server">(7)購案編號</asp:Label>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(2)軍品用途</asp:Label>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:TextBox ID="txtOVC_TARGET_DO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(8)交貨時間</asp:Label>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:TextBox ID="txtOVC_SHIP_TIMES_OUT" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(3)預算來源</asp:Label>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblONB_PUR_BUDGET_FORM_OUT" CssClass="control-label" runat="server"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(9)運輸方式</asp:Label>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:TextBox ID="txtOVC_WAY_TRANS" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(4)採購地區</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpOVC_COUNTRY" CssClass="tb tb-l" runat="server"></asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(10)起運及輸入口岸</asp:Label>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:TextBox ID="txtOVC_FROM_TO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(5)採購單位</asp:Label>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: left;">
                                                                 <asp:DropDownList ID="drpOVC_AGNT_IN_OUT" CssClass="tb tb-l" OnSelectedIndexChanged="drpOVC_AGNT_IN_OUT_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                                <asp:HiddenField ID="txt_OVC_DEPT_CDE2" runat="server"/>
                                                                <asp:Button ID="btnQuery" CssClass="btn-success btnw4" OnClick="btnQuery_Click" OnClientClick="OpenWindow('txt_OVC_DEPT_CDE2','txtOVC_AGNT_IN_SHOW')" runat="server" Text="單位查詢" />
                                                            </div>
                                                            <br />
                                                            <br />
                                                            <div style="text-align: left;">
                                                                <asp:TextBox ID="txtOVC_AGNT_IN_SHOW" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(11)接收地點</asp:Label>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:TextBox ID="txtOVC_TO_PLACE" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(6)接收單位</asp:Label>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:TextBox ID="txtOVC_RECEIVE_NSECTION" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(12)檢驗方法</asp:Label>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:TextBox ID="txtOVC_POI_IINSPECT_OUT" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="text-center">
                                                    <asp:Button ID="btnSave_plan" CssClass="btn-warning btnw2" OnClick="btnSave_plan_Click" runat="server" Text="存檔" />
                                                </div>
                                                <header class="title">
                                                    <!--標題-->
                                                    計劃清單編製
                                                </header>
                                                <table class="table table-bordered text-center">
                                                    <tr>
                                                        <td colspan="11" class="text-left">
                                                            <asp:Label CssClass="control-label" runat="server">(13)項次Item　　</asp:Label>
                                                            <asp:Label ID="lblONB_POI_ICOUNT" CssClass="control-label" runat="server">1</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td rowspan="5" style="width: 25px;">
                                                            <asp:Label CssClass="control-label" runat="server">(14)品名料號及規格Description</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">中文品名</asp:Label></td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:TextBox ID="txtOVC_POI_NSTUFF_CHN" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:Label CssClass="control-label" runat="server">英文品名</asp:Label></td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:TextBox ID="txtOVC_POI_NSTUFF_ENG" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">料號種類</asp:Label></td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:DropDownList ID="drpNSN_KIND" CssClass="tb tb-s" runat="server">
                                                                <asp:ListItem>請選擇</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">料號</asp:Label></td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:TextBox ID="txtNSN" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">廠牌</asp:Label></td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:TextBox ID="txtOVC_BRAND" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                            <asp:CheckBox ID="chkOVC_SAME_QUALITY_BRAND" runat="server" CssClass="radioButton" />(或同等品)
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">型號</asp:Label>
                                                        </td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:TextBox ID="txtOVC_MODEL" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                            <asp:CheckBox ID="chkOVC_SAME_QUALITY_MODEL" CssClass="radioButton" runat="server" />(或同等品)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">件號</asp:Label></td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:TextBox ID="txtOVC_POI_IREF" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                            <asp:CheckBox ID="chkOVC_SAME_QUALITY_POI_IREF" CssClass="rb-xs radioButton" runat="server" />(或同等品)
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">行政院財產分類編號</asp:Label>
                                                        </td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:TextBox ID="txtOVC_FCODE" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                            <asp:HiddenField ID="txtName" runat="server" />
                                                            <asp:Button ID="btnQuery_NUM" CssClass="btn-success btnw4" OnClick="btnQuery_NUM_Click" OnClientClick="OpenPhraseWindow('txtOVC_FCODE','txtName')" runat="server" Text="編號查詢" /><!--綠色-->
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">規格或性能</asp:Label>
                                                        </td>
                                                        <td colspan="9">
                                                            <asp:TextBox ID="txtOVC_INSPECT" TextMode="MultiLine" Rows="5" CssClass="text-left textarea tb-full" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label CssClass="control-label" runat="server">(15)單位Unit</asp:Label>
                                                        </td>
                                                        <td colspan="9" class="text-left">
                                                            <asp:DropDownList ID="drpOVC_POI_IUNIT" CssClass="tb tb-s" runat="server">
                                                                <asp:ListItem>EA個</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label CssClass="control-label" runat="server">(16)數量Qty</asp:Label>
                                                        </td>
                                                        <td colspan="9" class="text-left">
                                                            <asp:TextBox ID="txtONB_POI_QORDER_PLAN" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label CssClass="control-label" runat="server">(17)單價Unity Price</asp:Label>
                                                        </td>
                                                        <td colspan="9" class="text-left">
                                                            <asp:TextBox ID="txtONB_POI_MPRICE_PLAN" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" rowspan="3">
                                                            <asp:Label CssClass="control-label" runat="server">(18)備考Reference</asp:Label>
                                                        </td>
                                                        <td colspan="2" rowspan="3">
                                                            <asp:Label CssClass="control-label" runat="server">初次購買</asp:Label>
                                                        </td>
                                                        <td rowspan="3">
                                                            <asp:DropDownList ID="drpOVC_FIRST_BUY" CssClass="tb tb-s" runat="server">
                                                                <asp:ListItem Value="Y">Y:是</asp:ListItem>
                                                                <asp:ListItem Value="N">N:否</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td rowspan="2">
                                                            <asp:RadioButton ID="chk_OVC_NUM" GroupName="GroupBEFOUT" CssClass="radioButton" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">案號</asp:Label>
                                                        </td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:TextBox ID="txtOVC_POI_IPURCH_BEF" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">以往購價</asp:Label>
                                                        </td>
                                                        <td colspan="4" class="text-left">
                                                            <asp:Label CssClass="control-label" runat="server">數量</asp:Label>
                                                            <asp:TextBox ID="txtONB_POI_QORDER_BEF" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server">幣別</asp:Label>
                                                            <asp:DropDownList ID="drpOVC_CURR_MPRICE_BEF" CssClass="tb tb-s" runat="server">
                                                                <asp:ListItem>請選擇</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:Label CssClass="control-label" runat="server">金額</asp:Label>
                                                            <asp:TextBox ID="txtONB_POI_MPRICE_BEF" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="chk_OVC_SAME" GroupName="GroupBEFOUT" CssClass="radioButton" runat="server" />
                                                        </td>
                                                        <td colspan="5" class="text-left">
                                                            <asp:Label CssClass="control-label" runat="server">詳如商情資料</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label CssClass="control-label" runat="server">其它說明</asp:Label>
                                                        </td>
                                                        <td colspan="9" class="text-left">
                                                            <asp:TextBox ID="txtOVC_POI_NDESC" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="text-center">
                                                    <asp:Button ID="btnSave_DETIAL" CssClass="btn-warning" OnClick="btnSave_DETIAL_Click" runat="server" Text="採購明細存檔" /><!--黃色-->
                                                    <asp:Button ID="btnSave_PLANLIST" CssClass="btn-success" CommandName="PrintListOutPDF" OnCommand="btnPrint_Command" runat="server" Text="計劃清單預覽列印" /><!--黃色-->
                                                    <asp:Button ID="btnToDoc" CssClass="btn-success" runat="server" Text="通用條款及投標須知" OnClick="btnToDoc_Click" /><!--黃色-->
                                                </div>
                                                <div class="subtitle">採購明細結果顯示</div>
                                                <asp:GridView ID="GridView1" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false"  OnPreRender="GridView1_PreRender" ShowFooter="True" runat="server">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="項次" DataField="ONB_POI_ICOUNT" />
                                                        <asp:BoundField HeaderText="料號" DataField="NSN" />
                                                        <asp:BoundField HeaderText="中文品名" DataField="OVC_POI_NSTUFF_CHN" />
                                                        <asp:BoundField HeaderText="數量" DataField="ONB_POI_QORDER_PLAN" />
                                                        <asp:BoundField HeaderText="單價" DataField="ONB_POI_MPRICE_PLAN" />
                                                        <asp:TemplateField HeaderText="功能">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btn_change" OnClick="btn_change_Click" CssClass="btn-success btnw2" Text="異動" CommandName="按鈕屬性" runat="server" />
                                                                <asp:Button ID="btnDel" OnClick="btnDel_Click" CssClass="btn-danger btnw2" Text="刪除" CommandName="按鈕屬性" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <table class="table table-bordered text-center">
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <asp:Label CssClass="control-label" runat="server">(19)備註Remarks</asp:Label>
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <asp:Button ID="btnSave_OVC_MEMO" OnClick="btnSave_OVC_MEMO_Click" CssClass="btn-success btnw6" runat="server" Text="備註編輯" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <asp:Label CssClass="control-label" runat="server">(20)</asp:Label>
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <asp:Label ID="Label3" CssClass="control-label" runat="server">貸款總價：</asp:Label>
                                                            <asp:Label ID="lblOVC_GOOD_TOTAL" CssClass="control-label" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <asp:Label CssClass="control-label" runat="server">(21)附件</asp:Label>
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <asp:Button ID="btnFileUpload_Page2" CssClass="btn-success" runat="server" OnClick="btnFileUpload_Page2_Click" Text="附件及檔案上傳" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        </div>
                                        <%--複數決標案--計畫清單項目分組作業 PAGE3--%>
                                        <div id="third" class="tab-pane"><!--尚未選取頁面-->
                                             <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                            <asp:Panel ID="divThird" runat="server">
                                                <header class="title">
                                                    <!--標題-->
                                                    複數決標案--計畫清單項目分組作業
                                                </header>
                                                <div class="text-center">
                                                    <asp:Label CssClass="control-label text-blue" runat="server">購案編號：</asp:Label>
                                                    <asp:Label ID="lblPurchNum" CssClass="control-label text-blue" runat="server"></asp:Label><br>
                                                    <asp:Label CssClass="control-label text-blue" runat="server">(1)請先輸入要作業的組別：</asp:Label>
                                                    <asp:DropDownList ID="drpGROUP" CssClass="tb tb-xs" OnSelectedIndexChanged="drpGROUP_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList><br>
                                                </div>
                                                <div class="text-center">
                                                    <asp:Label CssClass="control-label text-red" runat="server">如果要修改第</asp:Label>
                                                    <asp:Label ID="lblONB_GROUP_PRE" CssClass="control-label text-red" runat="server">1</asp:Label>
                                                    <asp:Label CssClass="control-label text-red" runat="server">組項次請在此作業</asp:Label>
                                                </div>
                                                <table class="table table-bordered text-center">
                                                    <tr>
                                                        <th colspan="3">
                                                        <div class="text-left">
                                                        <asp:Label CssClass="control-label text-blue" runat="server">(2)組別：</asp:Label>
                                                        <asp:Label ID="lbldrpselect" CssClass="control-label text-blue" runat="server"></asp:Label>
                                                        </div>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:49%">
                                                            <asp:GridView ID="gvGroupLeft" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" OnPreRender="gvGroupLeft_PreRender" runat="server">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="作業">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="btnCancel_LEFT" CssClass="btn-danger btnw2" OnClick="btnCancel_LEFT_Click" Text="取消" CommandName="按鈕屬性" runat="server" />
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="項次" DataField="ONB_POI_ICOUNT" />
                                                                    <asp:BoundField HeaderText="品名" DataField="OVC_POI_NSTUFF_CHN" />
                                                                    <asp:BoundField HeaderText="廠牌/規格" DataField="OVC_BRAND" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                        <td style="width:2%">

                                                        </td>
                                                        <td style="width:49%">
                                                              <asp:GridView ID="gvGroupRight" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" OnPreRender="gvGroupRight_PreRender" runat="server">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="作業">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnCancel_Right" CssClass="btn-danger btnw2" OnClick="btnCancel_Right_Click" Text="取消" CommandName="按鈕屬性" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="項次" DataField="ONB_POI_ICOUNT" />
                                                        <asp:BoundField HeaderText="品名" DataField="OVC_POI_NSTUFF_CHN" />
                                                        <asp:BoundField HeaderText="廠牌/規格" DataField="OVC_BRAND" />
                                                    </Columns>
                                                </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left" colspan="3">
                                                            <asp:Label CssClass="control-label" runat="server">分組預算：</asp:Label>
                                                            <asp:Label ID="lblGRUOP_BUDGE" CssClass="control-label" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                
                                             
                                                <div class="text-center">
                                                    <asp:Label CssClass="control-label text-red" runat="server">如果要新增第</asp:Label>
                                                    <asp:Label ID ="lblONB_GROUP_PRE_2" CssClass="control-label text-red" runat="server">1</asp:Label>
                                                    <asp:Label CssClass="control-label text-red" runat="server">組分組項次請在此作業</asp:Label>
                                                </div>
                                                <table class="table table-bordered text-center">
                                                    <tr>
                                                        <th colspan="3">
                                                            <div class="text-left">
                                                            <asp:Label CssClass="control-label text-blue" runat="server">(2)計畫清單所有的項目：</asp:Label>
                                                            <asp:Button ID="btnNew" OnClick="btnNew_Click" CssClass="btn-success btnw4" runat="server" Text="新增內容" />
                                                            <asp:Button ID="btnReset" OnClick="btnReset_Click" CssClass="btn-danger btnw4" runat="server" Text="全部清除" />
                                                            <asp:Button ID="btnSelectAll" OnClick="btnSelectAll_Click" CssClass="btn-success btnw4" runat="server" Text="選擇全部" />
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:49%">
                                                            <asp:GridView ID="gvONB_POI_ICOUNT_LEFT" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="">
                                                                        <ItemTemplate>
                                                                            <asp:checkbox ID="cbIsGroupLeft" Visible='<%# (Eval("ONB_GROUP_PRE").ToString().Equals(string.Empty)) %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="項次" DataField="ONB_POI_ICOUNT" />
                                                                    <asp:BoundField HeaderText="組別" DataField="ONB_GROUP_PRE" />
                                                                    <asp:BoundField HeaderText="品名" DataField="OVC_POI_NSTUFF_CHN" />
                                                                    <asp:BoundField HeaderText="廠牌/規格" DataField="OVC_BRAND" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                        <td style="width:2%">
                                                        </td>
                                                        <td style="width:49%">
                                                            <asp:GridView ID="gvONB_POI_ICOUNT_Right" CssClass="table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="">
                                                                        <ItemTemplate>
                                                                            <asp:checkbox ID="cbIsGroupRight" Visible='<%# (Eval("ONB_GROUP_PRE").ToString().Equals(string.Empty)) %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="項次" DataField="ONB_POI_ICOUNT" />
                                                                    <asp:BoundField HeaderText="組別" DataField="ONB_GROUP_PRE" />
                                                                    <asp:BoundField HeaderText="品名" DataField="OVC_POI_NSTUFF_CHN" />
                                                                    <asp:BoundField HeaderText="廠牌/規格" DataField="OVC_BRAND" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>

                                              </table>
                                                
                                            </asp:Panel>
                                             </ContentTemplate>
                                        </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            <!--頁籤－結束-->
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>

