<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D19_1.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D19_1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
    </script>

    <div class="row">
        <div style="width: 1150px; margin:auto;">
            <section class="panel">
                <header class="title"><!--標題-->
                    購案開標紀錄作業編輯
                    <div style="text-align:right"><asp:LinkButton ID="lbtnVendor" OnClick="lbtnVendor_Click" class="subtitle" runat="server">廠商編輯</asp:LinkButton></div>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;" id="divForm" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <div id="tabs" class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <!--頁籤－開始-->
                            <header class="panel-heading">
                                <ul class="nav nav-tabs">
                                    <!--各頁籤-->
                                    <li class="active"><!--class="active" 起始選取頁籤-->
                                        <a data-toggle="tab" href="#TabFirst">第一頁</a> 
                                    </li>
                                    <li><!--尚未選取頁籤-->
                                        <a data-toggle="tab" href="#TabSecond">第二頁</a> 
                                    </li>
                                </ul>
                            </header>
                            <div class="panel-body tab-body">
                                <div class="tab-content">
                                    <!--各標籤之頁面-->
                                    <div id="TabFirst" class="tab-pane active"><!--起始選取頁面，只有一個-->
                                         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <table class="table table-bordered">
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server" >開標日期：</asp:Label>
                                                            <asp:Label ID="lblOVC_DOPEN" CssClass="control-label" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Label CssClass="control-label" runat="server" >開標地點：第</asp:Label>&nbsp;
                                                            <asp:TextBox ID="txtOVC_ROOM" CssClass="tb tb-xs" runat="server"></asp:TextBox>&nbsp;
                                                            <asp:Label CssClass="control-label" runat="server" >開標室</asp:Label>&nbsp;
                                                            <asp:Label CssClass="control-label" runat="server" >主標人：</asp:Label>&nbsp;
                                                            <asp:TextBox ID="txtOVC_CHAIRMAN" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server" >標的名稱及數量摘要：</asp:Label>
                                                            <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label>
                                                            <asp:Label CssClass="control-label" runat="server">(購案編號：</asp:Label>
                                                            <asp:Label ID="lblOVC_PURCH_A_5" CssClass="control-label" runat="server"></asp:Label>
                                                            <asp:Label CssClass="control-label text-red" runat="server">　第</asp:Label>
                                                            <asp:Label ID="lblONB_GROUP" CssClass="control-label text-red" runat="server"></asp:Label>
                                                            <asp:Label CssClass="control-label text-red" runat="server">組</asp:Label>
                                                            <asp:Label CssClass="control-label" runat="server">)</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server" >核准文號：</asp:Label>
                                                            <asp:Label ID="lblOVC_VEN_103_2" CssClass="control-label" runat="server"></asp:Label>
                                                            <asp:Label ID="lblOVC_PUR_APPROVE" CssClass="control-label" runat="server"></asp:Label>
                                                            <asp:Label CssClass="control-label" runat="server">核定書，刊登於</asp:Label>
                                                            <asp:Label ID="lblOVC_DANNOUNCE" CssClass="control-label" runat="server"></asp:Label>
                                                            <asp:Label CssClass="control-label" runat="server">政府採購公報。</asp:Label>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td><asp:Label runat="server" >主要內容記述：(下列有勾選者始為有效)</asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(一)本案採 </asp:Label>
                                                            <asp:Label ID="lblOVC_PUR_ASS_VEN_CODE" CssClass="control-label" runat="server"></asp:Label>
                                                            <asp:Label CssClass="control-label" runat="server" >(採依政府採購案第二十二條第一項  </asp:Label>
                                                            <asp:TextBox ID="txtOVC_22_1_NO" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server" >  款之</asp:Label>
                                                            <asp:RadioButtonList ID="rdoOVC_OPEN_METHOD" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:RadioButtonList>
                                                            <asp:Label CssClass="control-label" runat="server">，係第</asp:Label>
                                                            <asp:Label ID="lblONB_TIMES" CssClass="control-label" runat="server" ></asp:Label>
                                                            <asp:Label CssClass="control-label" runat="server">次開標為</asp:Label><br>
                                                            <asp:RadioButton ID="rdoOVC_BID_METHOD_1" OnCheckedChanged="rdoOVC_BID_METHOD_CheckedChanged" AutoPostBack="true" CssClass="radioButton" runat="server" Text="不分段標"/>。<br>
                                                            <asp:RadioButton ID="rdoOVC_BID_METHOD_2" OnCheckedChanged="rdoOVC_BID_METHOD_CheckedChanged" AutoPostBack="true" CssClass="radioButton" runat="server" Text="分段開標" />之
                                                            <asp:CheckBoxList ID="chkOVC_METHOD" OnSelectedIndexChanged="chkOVC_METHOD_SelectedIndexChanged" AutoPostBack="true" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem>資格標</asp:ListItem>
                                                                <asp:ListItem>規格標</asp:ListItem>
                                                                <asp:ListItem>價格標</asp:ListItem>
                                                            </asp:CheckBoxList><br>
                                                            <asp:RadioButton ID="rdoOVC_BID_METHOD_3" OnCheckedChanged="rdoOVC_BID_METHOD_CheckedChanged" AutoPostBack="true" CssClass="radioButton" runat="server" Text="最有利標"/>。<br>
                                                            <br><br>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(二)本案開標   </asp:Label>
                                                            <asp:CheckBoxList ID="chkOVC_RESTRICT" CssClass="radioButton" runat="server" RepeatLayout="UnorderedList">
                                                                <asp:ListItem>依政府採購法第四十八條第二項規定得不受三家廠商之限制</asp:ListItem>
                                                                <asp:ListItem Text="依「中央機關未達公告金額採購招標辦法」第三條規定經機關首長或其授權人員核准，改採限制性招標" Value="依「中央機關未達公告金額採購招標辦法」第三條規定經機關首長或其授權人員核准"></asp:ListItem>
                                                            </asp:CheckBoxList><br>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server" >(三)開、審標</asp:Label><br>
                                                            <asp:Label CssClass="control-label" runat="server" >本案投標廠商計 </asp:Label>
                                                            <asp:Label ID="lblONB_BID_VENDORS" CssClass="control-label" runat="server"></asp:Label>
                                                            <asp:Label CssClass="control-label" runat="server" >  家，含</asp:Label><br>
                                                            <asp:Label ID="lblONB_BID_VENDORS_Site" CssClass="control-label" runat="server"></asp:Label>
                                                            <asp:Label ID="lblOVC_VEN_TITLE_Site" CssClass="control-label" runat="server"></asp:Label>
                                                            
                                                            <asp:Label ID="lblONB_BID_VENDORS_Comm" CssClass="control-label" runat="server"></asp:Label>
                                                            <asp:Label ID="lblOVC_VEN_TITLE_Comm" CssClass="control-label" runat="server"></asp:Label>
                                                            
                                                            <asp:Label ID="lblONB_BID_VENDORS_Elec" CssClass="control-label" runat="server"></asp:Label>
                                                            <asp:Label ID="lblOVC_VEN_TITLE_Elec" CssClass="control-label" runat="server"></asp:Label>

                                                            <asp:Label CssClass="control-label" runat="server" >開標後審查如下：</asp:Label><br>
                                                            <asp:CheckBoxList ID="chkOVC_CHECK_RESULT" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem>資格</asp:ListItem>
                                                                <asp:ListItem>規格文</asp:ListItem>
                                                            </asp:CheckBoxList>
                                                            <asp:Label CssClass="control-label" runat="server" >審查結果：合格</asp:Label>
                                                            <asp:TextBox ID="txtONB_OK_VENDORS" CssClass="tb tb-xs" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server" >家。不合格</asp:Label>
                                                            <asp:TextBox ID="txtONB_NOTOK_VENDORS" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server" >家，  為</asp:Label>
                                                            <asp:DropDownList ID="drpOVC_VENDORS_NAME" OnSelectedIndexChanged="drpOVC_VENDORS_NAME_SelectedIndexChanged" AutoPostBack="true" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                                            <asp:TextBox ID="txtOVC_VENDORS_NAME" CssClass="tb tb-l" runat="server"></asp:TextBox><br><br>
                                                            <asp:CheckBoxList ID="chkOVC_AUDIT" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem>規格</asp:ListItem>
                                                                <asp:ListItem>建議書</asp:ListItem>
                                                                <asp:ListItem Text="" Value="Other"></asp:ListItem>
                                                            </asp:CheckBoxList>
                                                            <asp:TextBox ID="txtOVC_AUDIT_OTHER" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server" >，文件由委方攜回</asp:Label>
                                                            <asp:TextBox ID="txtONB_AUDIT_DOC" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server" >份審查，留存中心一份備查</asp:Label><br><br>
                                                            <asp:CheckBox ID="chkOVC_CHECK_DOC" CssClass="radioButton" Text="報價文件審查結果：" runat="server" />
                                                            
                                                            <asp:CheckBox ID="chkONB_OK_EFFECT" CssClass="radioButton" Text="有效標" runat="server" />
                                                            <asp:TextBox ID="txtONB_OK_EFFECT" CssClass="tb tb-xs" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server" >家。</asp:Label>
                                                            <asp:CheckBox ID="chkONB_NOTOK_EFFECT" CssClass="radioButton" Text="無效標" runat="server" />
                                                            <asp:TextBox ID="txtONB_NOTOK_EFFECT" CssClass="tb tb-xs" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server" >家，為</asp:Label>
                                                            <asp:DropDownList ID="drpOVC_EFFECTS_NAME" OnSelectedIndexChanged="drpOVC_EFFECTS_NAME_SelectedIndexChanged" AutoPostBack="true" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                                            <asp:TextBox ID="txtOVC_EFFECTS_NAME" CssClass="tb tb-l" runat="server"></asp:TextBox><br><br>
                                                            <asp:Label CssClass="control-label" runat="server" >審標結果：</asp:Label>
                                                            <asp:TextBox ID="txtONB_RESULT_OK" CssClass="tb tb-xs" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server" >家符合招標文件規定，其餘</asp:Label>
                                                            <asp:TextBox ID="txtONB_RESULT_NOTOK" CssClass="tb tb-xs" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server" >家不合格</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server" >(四)流、廢標</asp:Label><br>
                                                            <asp:RadioButton ID="rdoOVC_RESULT_1" CssClass="radioButton text-red" Text="流標原因：" OnCheckedChanged="rdoOVC_RESULT_CheckedChanged" AutoPostBack="true" runat="server"/>
                                                            <asp:Label CssClass="control-label" runat="server" >投標商未達法定家數(</asp:Label>
                                                            <asp:TextBox ID="txtONB_BID_VENDOR_LAW" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server" >家投標)</asp:Label>
                                                            <asp:CheckBox ID="chkOVC_BACK" CssClass="radioButton" Text="廠商投標文件予以發還各廠商" runat="server" /><br>
                                                            <asp:RadioButton ID="rdoOVC_RESULT_2"  CssClass="radioButton text-red" Text="廢標原因：" OnCheckedChanged="rdoOVC_RESULT_CheckedChanged" AutoPostBack="true" runat="server" />
                                                            <asp:RadioButtonList ID="rdoOVC_RESULT_REASON_2" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                                                <asp:ListItem>最低報價超底價</asp:ListItem>
                                                                <asp:ListItem>最低報價超預算</asp:ListItem>
                                                                <asp:ListItem>最低報價逾評審委員會建議金額</asp:ListItem>
                                                                <asp:ListItem>經審標結果，無得為決標對象之廠商</asp:ListItem>
                                                                <asp:ListItem>投標未達法定家數</asp:ListItem>
                                                                <asp:ListItem>物資接轉處進口承辦人</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server">(五)</asp:Label>
                                                            <asp:RadioButton ID="rdoOVC_RESULT_0" CssClass="radioButton text-red" Text="決標" OnCheckedChanged="rdoOVC_RESULT_CheckedChanged" AutoPostBack="true" runat="server" /><br>
                                                            <asp:DropDownList ID="drpOVC_VEN_CST_0" CssClass="tb tb-m" OnSelectedIndexChanged="drpOVC_VEN_CST_0_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                            <asp:TextBox ID="txtOVC_VEN_TITLE_0" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server">報價(減價後)</asp:Label>
                                                            <asp:RadioButton ID="rdoOVC_FOLLOW_OK_Y" CssClass="radioButton" Text="按底價承製" OnCheckedChanged="rdoOVC_FOLLOW_OK_CheckedChanged" AutoPostBack="true" runat="server" />
                                                            <asp:RadioButton ID="rdoOVC_FOLLOW_OK_N" CssClass="radioButton" OnCheckedChanged="rdoOVC_FOLLOW_OK_CheckedChanged" AutoPostBack="true" runat="server" />
                                                            <asp:DropDownList ID="drpOVC_BID_CURRENT_0" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                                            <asp:TextBox ID="txtONB_BID_MONEY_0" CssClass="tb tb-s" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server">元整最低</asp:Label><br><br>
                                                            <asp:Label CssClass="control-label" runat="server">，且在</asp:Label>
                                                            <asp:RadioButton ID="rdoOVC_RESULT_REASON_0_0" CssClass="radioButton" Text="底價" OnCheckedChanged="rdoOVC_RESULT_REASON_0_CheckedChanged" AutoPostBack="true" runat="server" />
                                                            <asp:DropDownList ID="drpOVC_CURRENT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                                            <asp:TextBox ID="txtONB_BID_BUDGET" CssClass="tb tb-s" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server">元(含)以內</asp:Label>
                                                            <asp:RadioButton ID="rdoOVC_RESULT_REASON_0_1" CssClass="radioButton" Text="評審委員會認為廠商報價合理且在預算以內" OnCheckedChanged="rdoOVC_RESULT_REASON_0_CheckedChanged" AutoPostBack="true" runat="server" />，<br><br>
                                                            <asp:RadioButton ID="rdoONB_COMMITTEE_BUDGET" CssClass="radioButton" Text="評審委員會建議金額" OnCheckedChanged="rdoOVC_RESULT_REASON_0_CheckedChanged" AutoPostBack="true" runat="server" />
                                                            <asp:DropDownList ID="drpOVC_COMMITTEE_CURRENT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                                            <asp:TextBox ID="txtONB_COMMITTEE_BUDGET" CssClass="tb tb-s" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server" >元(含)以內</asp:Label>
                                                            <asp:RadioButton ID="rdoOVC_RESULT_REASON_0_2" CssClass="radioButton" Text="評選委員會依規定評選為最有利標" OnCheckedChanged="rdoOVC_RESULT_REASON_0_CheckedChanged" AutoPostBack="true" runat="server" />，<br><br>
                                                            <asp:TextBox ID="txtOVC_RESULT_DESC" TextMode="MultiLine" Rows="2" CssClass="textarea tb-full" runat="server"></asp:TextBox><br>
                                                            <asp:Label CssClass="control-label" runat="server" >依政府採購法第五十二條第</asp:Label>
                                                            <asp:TextBox ID="txtOVC_LAW_ITEM" CssClass="tb tb-xs" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server" >項 第</asp:Label>
                                                            <asp:TextBox ID="txtOVC_LAW_NO" CssClass="tb tb-xs" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server">款決標。</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server" >(六)</asp:Label>
                                                            <asp:RadioButton ID="rdoOVC_RESULT_3" CssClass="radioButton text-red" Text="保留開標結果" OnCheckedChanged="rdoOVC_RESULT_CheckedChanged" AutoPostBack="true" runat="server" />
                                                            <asp:DropDownList ID="drpOVC_VEN_CST_3" CssClass="tb tb-m" OnSelectedIndexChanged="drpOVC_VEN_CST_3_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                            <asp:TextBox ID="txtOVC_VEN_TITLE_3" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server" >報價(減價後)</asp:Label>
                                                            <asp:DropDownList ID="drpOVC_BID_CURRENT_3" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                                            <asp:TextBox ID="txtONB_BID_MONEY_3" CssClass="tb tb-s" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server" >元整最低，</asp:Label><br><br>
                                                            <asp:Label CssClass="control-label" runat="server" >進入底價，因預算待核定，保留開標結果；俟預算奉核後另行辦理決標，否則廢標，如有押標金則無息退還。</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="control-label" runat="server" Text="(七)"></asp:Label>
                                                            <asp:RadioButton ID="rdoOVC_RESULT_4" CssClass="radioButton text-red" Text="不予開標、決標" OnCheckedChanged="rdoOVC_RESULT_CheckedChanged" AutoPostBack="true" runat="server"/><br><br>
                                                            <asp:Label CssClass="control-label" runat="server" >因</asp:Label>
                                                            <asp:RadioButtonList ID="rdoOVC_RESULT_REASON_4" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                                                <asp:ListItem>廠商異議或申訴事項審議</asp:ListItem>
                                                                <asp:ListItem>採購計畫變更</asp:ListItem>
                                                                <asp:ListItem>委方取消採購</asp:ListItem>
                                                                <asp:ListItem>發現有足以影響採購公正之違法或不當行為</asp:ListItem>
                                                            </asp:RadioButtonList><br><br>
                                                            <asp:Label CssClass="control-label" runat="server">，經主持人當場依政府採購案法第四十八條第一項第</asp:Label>
                                                            <asp:TextBox ID="txtOVC_NONE_BID_NO" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                            <asp:Label CssClass="control-label" runat="server">款宣布不予開標、決標。</asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table class="table table-bordered">
                                                    <tr>
                                                        <td colspan="3" style="text-align:right;Width:50%;border-right:none">
                                                            <asp:Label CssClass="control-label text-red" runat="server">主官核批日：</asp:Label>
                                                        </td>
                                                        <td colspan="3" style="text-align:left;Width:50%;border-left: none;">
                                                            <div class="input-append datepicker position-left" style="border-left-style:none;border-left:none">
                                                                <asp:TextBox ID="txtOVC_DAPPROVE" CssClass="tb tb-s position-left text-change" runat="server"></asp:TextBox>
                                                                <span class='add-on'><i class="icon-calendar"></i></span>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>



                                    <div id="TabSecond" class="tab-pane"><!--尚未選取頁面-->
                                         <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <div class="text-center">
                                                    <asp:Label CssClass="control-label" runat="server" Font-Size="Large">購案編號：</asp:Label>
                                                    <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server" Font-Size="Large"></asp:Label>
                                                    <asp:Label CssClass="control-label text-red" runat="server" Font-Size="Large"> （第</asp:Label>
                                                    <asp:Label ID="lblONB_GROUP_TBM1313" CssClass="control-label text-red" runat="server" Font-Size="Large"></asp:Label>
                                                    <asp:Label CssClass="control-label text-red" runat="server" Font-Size="Large">組）</asp:Label><br>
                                                </div>


                                                <!-- 投標廠商報價比價表 -->
                                                <asp:GridView ID="gvTBM1314" OnRowCommand="gvTBM1314_RowCommand" CssClass="table data-table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
 		     		                                <Columns>
                                                        <asp:TemplateField HeaderText="作業">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnChange" CommandName="Chnage" CssClass="btn-default btnw2" Text="異動" runat="server" />
                                                                <asp:Button ID="btnDel" CommandName="Del" CssClass="btn-warning btnw2" Text="刪除" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server" /><br />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="組別" DataField="ONB_GROUP" ItemStyle-CssClass="text-center" />
                                                        <asp:TemplateField HeaderText="廠商名稱">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOVC_VEN_CST" Text='<%# Eval("OVC_VEN_CST") %>' CssClass="control-label" Style="display:none" runat="server"></asp:Label>
                                                               <asp:Label ID="lblOVC_VEN_TITLE" Text='<%# Eval("OVC_VEN_TITLE") %>' CssClass="control-label" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="標價" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblONB_MBID" Text='<%# Bind("ONB_MBID") %>' CssClass="control-label" runat="server"></asp:Label><br />
                                                                <asp:Label ID="lblOVC_MBID" Text='<%# Eval("OVC_MBID") %>' CssClass="control-label" runat="server"></asp:Label><br />
                                                                <asp:Label ID="lblOVC_KMBID" Text='<%# Eval("OVC_KMBID") %>' CssClass="control-label" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="最低標優先減" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblONB_MINIS_LOWEST" Text='<%# Bind("ONB_MINIS_LOWEST") %>' CssClass="control-label" runat="server"></asp:Label><br />
                                                                <asp:Label ID="lblOVC_MINIS_LOWEST" Text='<%# Eval("OVC_MINIS_LOWEST") %>' CssClass="control-label" runat="server"></asp:Label><br />
                                                                <asp:Label ID="lblOVC_KMINIS_LOWEST" Text='<%# Eval("OVC_KMINIS_LOWEST") %>' CssClass="control-label" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="第一次比減價" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblONB_MINIS_1" Text='<%# Bind("ONB_MINIS_1") %>' CssClass="control-label" runat="server"></asp:Label><br />
                                                                <asp:Label ID="lblOVC_MINIS_1" Text='<%# Eval("OVC_MINIS_1") %>' CssClass="control-label" runat="server"></asp:Label><br />
                                                                <asp:Label ID="lblOVC_KMINIS_1" Text='<%# Eval("OVC_KMINIS_1") %>' CssClass="control-label" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="第二次比減價" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblONB_MINIS_2" Text='<%# Bind("ONB_MINIS_2") %>' CssClass="control-label" runat="server"></asp:Label><br />
                                                                <asp:Label ID="lblOVC_MINIS_2" Text='<%# Eval("OVC_MINIS_2") %>' CssClass="control-label" runat="server"></asp:Label><br />
                                                                <asp:Label ID="lblOVC_KMINIS_2" Text='<%# Eval("OVC_KMINIS_2") %>' CssClass="control-label" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="第三次比減價" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblONB_MINIS_3" Text='<%# Bind("ONB_MINIS_3") %>' CssClass="control-label" runat="server"></asp:Label><br />
                                                                <asp:Label ID="lblOVC_MINIS_3" Text='<%# Eval("OVC_MINIS_3") %>' CssClass="control-label" runat="server"></asp:Label><br />
                                                                <asp:Label ID="lblOVC_KMINIS_3" Text='<%# Eval("OVC_KMINIS_3") %>' CssClass="control-label" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="更多" ItemStyle-CssClass="text-center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnMore" CommandName="More" CssClass="btn-default btnw2" Text="..." runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
	   		                                    </asp:GridView>


                                                <table class="table table-bordered text-center">
                                                    <tr>
                                                        <td><asp:Button ID="btnSave_TBM1314" OnClick="btnSave_TBM1314_Click" CssClass="btn-default btnw2" Text="存檔" runat="server" /></td>
                                                        <td><asp:TextBox ID="txtONB_GROUP_TBM1314" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                        <td>
                                                            <asp:DropDownList ID="drpOVC_VEN_TITLE_TBM1314" OnSelectedIndexChanged="drpOVC_VEN_TITLE_SelectedIndexChanged" AutoPostBack="true" CssClass="tb tb-s" runat="server"></asp:DropDownList><br>
                                                            <asp:TextBox ID="txtOVC_VEN_CST_TBM1314" CssClass="tb tb-s" runat="server"></asp:TextBox><br>
                                                            <asp:TextBox ID="txtOVC_VEN_TITLE_TBM1314" CssClass="tb tb-s" runat="server"></asp:TextBox><br>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:Label CssClass="control-label text-red" runat="server">標價類別(擇一)</asp:Label><br>
                                                            <asp:RadioButtonList ID="rdoOVC_MBID" CssClass="radioButton text-red" RepeatLayout="UnorderedList" runat="server" >
                                                                <asp:ListItem Text="一般(單價)" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="一般(總價)" Value="5"></asp:ListItem>
                                                                <asp:ListItem Text="折扣率" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="不再減價" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="按底價承製" Value="4"></asp:ListItem>
                                                            </asp:RadioButtonList><br><br>
                                                            <asp:Label CssClass="control-label text-red" runat="server" >金額或折扣率</asp:Label><br>
                                                            <asp:TextBox ID="txtONB_MBID" CssClass="tb tb-s" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox><br><br>
                                                            <asp:Label CssClass="control-label" runat="server" >標價區分(擇一)</asp:Label><br>
                                                            <asp:RadioButtonList ID="rdoOVC_KMBID" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                                                <asp:ListItem Text="決" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="廢" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="保留" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="並列" Value="4"></asp:ListItem>
                                                                <asp:ListItem Text="無效標" Value="5"></asp:ListItem>
                                                                <asp:ListItem Text="不合格標" Value="7"></asp:ListItem>
                                                                <asp:ListItem Text="以上皆非" Value="6"></asp:ListItem>
                                                            </asp:RadioButtonList><br><br>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:Label CssClass="control-label text-red" runat="server">標價類別(擇一)</asp:Label><br>
                                                            <asp:RadioButtonList ID="rdoOVC_MINIS_LOWEST" CssClass="radioButton text-red" RepeatLayout="UnorderedList" runat="server" >
                                                                <asp:ListItem Text="一般(單價)" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="一般(總價)" Value="5"></asp:ListItem>
                                                                <asp:ListItem Text="折扣率" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="不再減價" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="按底價承製" Value="4"></asp:ListItem>
                                                            </asp:RadioButtonList><br><br>
                                                            <asp:Label CssClass="control-label text-red" runat="server" >金額或折扣率</asp:Label><br>
                                                            <asp:TextBox ID="txtONB_MINIS_LOWEST" CssClass="tb tb-s" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox><br><br>
                                                            <asp:Label CssClass="control-label" runat="server" >標價區分(擇一)</asp:Label><br>
                                                            <asp:RadioButtonList ID="rdoOVC_KMINIS_LOWEST" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                                                <asp:ListItem Text="決" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="廢" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="保留" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="並列" Value="4"></asp:ListItem>
                                                                <asp:ListItem Text="無效標" Value="5"></asp:ListItem>
                                                                <asp:ListItem Text="不合格標" Value="7"></asp:ListItem>
                                                                <asp:ListItem Text="以上皆非" Value="6"></asp:ListItem>
                                                            </asp:RadioButtonList><br><br>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:Label CssClass="control-label text-red" runat="server">標價類別(擇一)</asp:Label><br>
                                                            <asp:RadioButtonList ID="rdoOVC_MINIS_1" CssClass="radioButton text-red" RepeatLayout="UnorderedList" runat="server" >
                                                                <asp:ListItem Text="一般(單價)" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="一般(總價)" Value="5"></asp:ListItem>
                                                                <asp:ListItem Text="折扣率" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="不再減價" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="按底價承製" Value="4"></asp:ListItem>
                                                            </asp:RadioButtonList><br><br>
                                                            <asp:Label CssClass="control-label text-red" runat="server" >金額或折扣率</asp:Label><br>
                                                            <asp:TextBox ID="txtONB_MINIS_1" CssClass="tb tb-s" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox><br><br>
                                                            <asp:Label CssClass="control-label" runat="server" >標價區分(擇一)</asp:Label><br>
                                                            <asp:RadioButtonList ID="rdoOVC_KMINIS_1" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                                                <asp:ListItem Text="決" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="廢" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="保留" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="並列" Value="4"></asp:ListItem>
                                                                <asp:ListItem Text="無效標" Value="5"></asp:ListItem>
                                                                <asp:ListItem Text="不合格標" Value="7"></asp:ListItem>
                                                                <asp:ListItem Text="以上皆非" Value="6"></asp:ListItem>
                                                            </asp:RadioButtonList><br><br>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:Label CssClass="control-label text-red" runat="server">標價類別(擇一)</asp:Label><br>
                                                            <asp:RadioButtonList ID="rdoOVC_MINIS_2" CssClass="radioButton text-red" RepeatLayout="UnorderedList" runat="server" >
                                                                <asp:ListItem Text="一般(單價)" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="一般(總價)" Value="5"></asp:ListItem>
                                                                <asp:ListItem Text="折扣率" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="不再減價" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="按底價承製" Value="4"></asp:ListItem>
                                                            </asp:RadioButtonList><br><br>
                                                            <asp:Label CssClass="control-label text-red" runat="server" >金額或折扣率</asp:Label><br>
                                                            <asp:TextBox ID="txtONB_MINIS_2" CssClass="tb tb-s" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox><br><br>
                                                            <asp:Label CssClass="control-label" runat="server" >標價區分(擇一)</asp:Label><br>
                                                            <asp:RadioButtonList ID="rdoOVC_KMINIS_2" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                                                <asp:ListItem Text="決" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="廢" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="保留" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="並列" Value="4"></asp:ListItem>
                                                                <asp:ListItem Text="無效標" Value="5"></asp:ListItem>
                                                                <asp:ListItem Text="不合格標" Value="7"></asp:ListItem>
                                                                <asp:ListItem Text="以上皆非" Value="6"></asp:ListItem>
                                                            </asp:RadioButtonList><br><br>
                                                        </td>
                                                        <td class="text-left">
                                                            <asp:Label CssClass="control-label text-red" runat="server">標價類別(擇一)</asp:Label><br>
                                                            <asp:RadioButtonList ID="rdoOVC_MINIS_3" CssClass="radioButton text-red" RepeatLayout="UnorderedList" runat="server" >
                                                                <asp:ListItem Text="一般(單價)" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="一般(總價)" Value="5"></asp:ListItem>
                                                                <asp:ListItem Text="折扣率" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="不再減價" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="按底價承製" Value="4"></asp:ListItem>
                                                            </asp:RadioButtonList><br><br>
                                                            <asp:Label CssClass="control-label text-red" runat="server">金額或折扣率</asp:Label><br>
                                                            <asp:TextBox ID="txtONB_MINIS_3" CssClass="tb tb-s" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox><br><br>
                                                            <asp:Label CssClass="control-label" runat="server" >標價區分(擇一)</asp:Label><br>
                                                            <asp:RadioButtonList ID="rdoOVC_KMINIS_3" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                                                <asp:ListItem Text="決" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="廢" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="保留" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="並列" Value="4"></asp:ListItem>
                                                                <asp:ListItem Text="無效標" Value="5"></asp:ListItem>
                                                                <asp:ListItem Text="不合格標" Value="7"></asp:ListItem>
                                                                <asp:ListItem Text="以上皆非" Value="6"></asp:ListItem>
                                                            </asp:RadioButtonList><br><br>
                                                        </td>
                                                    </tr>
                                                </table>
                                                
                                                

                                                <!-- 得標廠商 -->
                                                <div class="subtitle"><asp:Label CssClass="control-label" runat="server">得標廠商：</asp:Label></div>
                                                <asp:GridView ID="gvTBM1302" OnRowCommand="gvTBM1302_RowCommand" CssClass=" table data-table table-striped border-top text-center" AutoGenerateColumns="false" runat="server">
 		     		                                <Columns>
                                                        <asp:TemplateField HeaderText="作業">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnChange" CommandName="Chnage" CssClass="btn-default btnw2" Text="異動" runat="server" />
                                                                <asp:Button ID="btnDel" CommandName="Del" CssClass="btn-warning btnw2" Text="刪除" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="組別">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblONB_GROUP" Text='<%# Eval("ONB_GROUP") %>' CssClass="control-label" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="統一編號">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOVC_VEN_CST" Text='<%# Eval("OVC_VEN_CST") %>' CssClass="control-label" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="廠商名稱">
                                                            <ItemTemplate>
                                                               <asp:Label ID="lblOVC_VEN_TITLE" Text='<%# Eval("OVC_VEN_TITLE") %>' CssClass="control-label" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="廠商電話">
                                                            <ItemTemplate>
                                                               <asp:Label ID="lblOVC_VEN_TEL" Text='<%# Eval("OVC_VEN_TEL") %>' CssClass="control-label" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="契約號">
                                                            <ItemTemplate>
                                                               <asp:Label ID="lblOVC_PURCH_6" Text='<%# Eval("OVC_PURCH_6") %>' CssClass="control-label" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        </Columns>
	   		                                    </asp:GridView>


                                                <table class="table table-bordered text-center">
                                                    <tr>
                                                        <td><asp:Button ID="btnSave_TBM1302" CssClass="btn-default btnw2" Text="存檔" OnClick="btnSave_TBM1302_Click" runat="server" /></td>
                                                        <td><asp:TextBox ID="txtONB_GROUP_TBM1302" CssClass="tb tb-xs" runat="server"></asp:TextBox></td>
                                                        <td>
                                                            <asp:DropDownList ID="drpOVC_VEN_TITLE_TBM1302" OnSelectedIndexChanged="drpOVC_VEN_TITLE_SelectedIndexChanged" AutoPostBack="true" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                                            <asp:TextBox ID="txtOVC_VEN_CST_TBM1302" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td><asp:TextBox ID="txtOVC_VEN_TITLE_TBM1302" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                        <td><asp:TextBox ID="txtOVC_VEN_TEL" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                        <td><asp:TextBox ID="txtOVC_PURCH_6" Text="PE" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                </table>

                                                <div>
                                                    <asp:Label CssClass="control-label" runat="server">決標原則：依政府採購法第五十二條第</asp:Label>
                                                    <asp:Label ID="lblOVC_LAW_ITEM" CssClass="control-label" runat="server"></asp:Label>
                                                    <asp:Label runat="server" CssClass="control-label" >項第</asp:Label>
                                                    <asp:Label ID="lblOVC_LAW_NO" CssClass="control-label" runat="server"></asp:Label>
                                                    <asp:Label CssClass="control-label" runat="server" >款。</asp:Label><br>

                                                    <asp:Label CssClass="control-label" runat="server" >決標金額：</asp:Label>
                                                    <asp:DropDownList ID="drpOVC_RESULT_CURRENT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                                    <asp:TextBox ID="txtONB_BID_RESULT" CssClass="tb tb-s" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                                    <asp:Label CssClass="control-label" runat="server" >元整。</asp:Label><br>

                                                    <asp:Label CssClass="control-label text-red" runat="server" >補充記載：(如有特殊情況或複數決標案，請分項列計)</asp:Label>
                                                    <asp:TextBox ID="txtOVC_ADDITIONAL" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox><br>

                                                    <asp:Label CssClass="control-label" runat="server" >應完成事項：</asp:Label>
                                                    <asp:CheckBoxList ID="chkOVC_FINISH" CssClass="radioButton" runat="server">
                                                        <asp:ListItem>續辦招標</asp:ListItem>
                                                        <asp:ListItem>本紀錄當場分發不另行文</asp:ListItem>
                                                    </asp:CheckBoxList><br>

                                                    <asp:Label CssClass="control-label" runat="server" >其他：</asp:Label>
                                                    <asp:TextBox ID="txtOVC_ADVICE" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <!--頁籤結束-->
                        </div><br />
                        <div style="text-align:center">
                            <asp:Button ID="btnSave" OnClick="btnSave_TBM1303_Click" CssClass="btn-default btnw2" Text="存檔" runat="server" />　　
                            <asp:Button ID="btnReturn" CssClass="btn-default" Text="回上一頁" OnClick="btnReturn_Click" runat="server" />
                        </div><br />
                        <div style="text-align:center">
                            <asp:Button ID="btnTBMBID_BACK" CssClass="btn-default" Text="押標金投標文件退還作業" OnClick="btnTBMBID_BACK_Click" runat="server" />　　
                            <asp:Button ID="btnTBMBID_DOC_LOG" CssClass="btn-default" Text="開標紀錄分送作業" OnClick="btnTBMBID_DOC_LOG_Click" runat="server" />
                        </div><br />
                        <div>
                            <div class="subtitle">預覽列印</div>
                            <div class="text-center diva">
                                <asp:LinkButton id="lbtnToWordD19_1_1" OnClick="lbtnToWordD19_1_1_Click" runat="server">開、決標、比議價紀錄</asp:LinkButton>
                                <asp:LinkButton id="lbtnToWordD19_1_2" OnClick="lbtnToWordD19_1_2_Click" runat="server">廠商減價單(分組)</asp:LinkButton>
                                <asp:LinkButton id="lbtnToWordD19_1_3" OnClick="lbtnToWordD19_1_3_Click" runat="server">廠商減價單(折扣)</asp:LinkButton>
                                <asp:LinkButton id="lbtnToWordD19_1_4" OnClick="lbtnToWordD19_1_4_Click" runat="server">廠商減價單(總價)</asp:LinkButton>
                                <asp:LinkButton id="lbtnToWordD19_1_5" OnClick="lbtnToWordD19_1_5_Click" runat="server">減價單(單價)</asp:LinkButton><br /><br />

                                <asp:LinkButton id="lbtnToWordD19_1_6" OnClick="lbtnToWordD19_1_6_Click" runat="server">開標結果通知押標金投標文件退還紀錄表</asp:LinkButton>
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

