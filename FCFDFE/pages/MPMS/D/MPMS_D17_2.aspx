<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D17_2.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D17_2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <style type="text/css">
        .rdoOVC_RESULT_Item1 input[type=radio]:checked + label {color:blue;}
        .rdoOVC_RESULT_Item2 input[type=radio]:checked + label {color:red;}
        .rdoOVC_RESULT_Item3 input[type=radio]:checked + label {color:green;}
    </style>

    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
    </script>




    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title"><!--標題-->
                    購案開標通知作業編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;" id="divForm" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <div id="tabs" class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <!--頁籤－開始-->
                            <header class="panel-heading">
                                <ul class="nav nav-tabs" ID="myTabs">
                                    <!--各頁籤-->
                                    <li ID="pageHome" class="active">
                                        <a data-toggle="tab" href="#TabHome">主頁面</a>
                                    </li>
                                    <li ID="pageAnnouncement">
                                        <a data-toggle="tab" href="#TabAnnouncement">是否公告</a>
                                    </li>
                                    <li ID="pageCheck">
                                        <a data-toggle="tab" href="#TabCheck">檢查項目明細</a>
                                    </li>
                                </ul>
                            </header>
                            <div class="panel-body tab-body">
                                <div class="tab-content">
                                    <!--各標籤之頁面-->
                                    <div id="TabHome" class="tab-pane active"><!--起始選取頁面-->
                                    <!-- 主畫面  -->
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <table class="table table-bordered" style="text-align:center">
                                                    <tr>
                                                        <td style="width:10%">
                                                            <asp:Label CssClass="control-label" Text="購案編號" runat="server"></asp:Label></td>
                                                        <td style="width:40%" class="text-left">
                                                            <asp:Label ID="lblOVC_PURCH_A_5" CssClass="control-label" runat="server"></asp:Label>
                                                            <asp:Label ID="lblOVC_PURCH" Style="display:none" CssClass="control-label" runat="server"></asp:Label>
                                                            <asp:Label ID="lblOVC_PUR_AGENCY" Style="display:none" CssClass="control-label" runat="server"></asp:Label>
                                                            <asp:Label ID="lblOVC_PURCH_5" Style="display:none"  CssClass="control-label" runat="server"></asp:Label></td>   
                                                        <td style="width:10%">
                                                            <asp:Label CssClass="control-label" Text="購案名稱" runat="server"></asp:Label></td>
                                                        <td style="width:40%" class="text-left">
                                                            <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                                    </tr>
                                                    <tr class="no-bordered-seesaw">
                                                        <td><asp:Label CssClass="control-label" Text="開標時間" runat="server"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblOVC_DOPEN" CssClass="control-label" runat="server"></asp:Label>&nbsp;
                                                            <asp:Label ID="lblOVC_OPEN_HOUR" CssClass="control-label" runat="server"></asp:Label>
                                                            <asp:Label CssClass="control-label" runat="server">時</asp:Label>
                                                            <asp:Label ID="lblOVC_OPEN_MIN" CssClass="control-label" runat="server"></asp:Label>
                                                            <asp:Label CssClass="control-label" runat="server">分 (第</asp:Label>
                                                            <asp:Label ID="lblONB_TIMES" CssClass="control-label text-red" runat="server"></asp:Label>
                                                            <asp:Label CssClass="control-label" runat="server">次) </asp:Label>
                                                            <asp:Label CssClass="control-label text-red" runat="server">組別：</asp:Label>
                                                            <asp:Label ID="lblONB_GROUP" CssClass="control-label text-red" runat="server"></asp:Label>
                                                        </td>
                                                        <td><asp:Label CssClass="control-label" Text="投標段次" runat="server"></asp:Label></td>
                                                        <td class="text-left"><asp:Label ID="lblOVC_BID_TIMES" CssClass="control-label" runat="server"></asp:Label></td>
                                                    </tr>
                                                    <tr class="no-bordered-seesaw">
                                                        <td><asp:Label CssClass="control-label" Text="決標原則" runat="server"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblOVC_BID_METHOD_1" CssClass="control-label" runat="server"></asp:Label>,
                                                            <asp:Label ID="lblOVC_BID_METHOD_2" CssClass="control-label" runat="server"></asp:Label>,
                                                            <asp:Label ID="lblOVC_BID_METHOD_3" CssClass="control-label" runat="server"></asp:Label></td>
                                                        <td><asp:Label CssClass="control-label" Text="招標方式" runat="server"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label CssClass="control-label" runat="server">(原：</asp:Label>
                                                            <asp:Label ID="lblOVC_PUR_ASS_VEN_CODE" CssClass="control-label" runat="server"></asp:Label>
                                                            <asp:Label CssClass="control-label" runat="server">)　</asp:Label>
                                                            <asp:Label CssClass="control-label text-red" runat="server">簽辦：</asp:Label>
                                                            <asp:Label ID="lblOVC_PUR_ASS_VEN_CODE_Sign" CssClass="control-label text-red" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>


                                    <div id="TabAnnouncement" class="tab-pane">
                                    <!-- 是否公告  -->
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <table style="text-align:center">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoOVC_BID_OPEN" OnSelectedIndexChanged="rdoOVC_BID_OPEN_SelectedIndexChanged" AutoPostBack="true" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                                                <asp:ListItem Value="Y" Text="公告"></asp:ListItem>
                                                                <asp:ListItem Value="N" Text="不公告"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoOVC_KIND" Visible="false" OnSelectedIndexChanged="rdoOVC_KIND_SelectedIndexChanged" AutoPostBack="true" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                                                <asp:ListItem Value="B" Text="選擇性"></asp:ListItem>
                                                                <asp:ListItem Value="C" Text="限制性"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table><br />

                                                <div id="divVendor" visible="false" runat="server">
                                                    <div style="text-align:center">
                                                        <asp:Label CssClass="control-label text-red" runat="server">招標廠商編輯</asp:Label>
                                                        <asp:Label CssClass="control-label text-blue" runat="server">(廠商資料存檔後列印邀商通知函)</asp:Label>
                                                    </div>
                                                    <asp:GridView ID="gvBID_VENDOR" CssClass="table data-table table-striped border-top text-center" OnRowCommand="gvBID_VENDOR_RowCommand" AutoGenerateColumns="False" runat="server">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="作業">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnChange" CssClass="btn-default btnw2" Text="異動" CommandName="Modify" runat="server" />
                                                                    <asp:Button ID="btnDel" CssClass="btn-warning btnw2" Text="刪除" CommandName="Del" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="統一編號">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOVC_VEN_CST" Text='<%# Bind("OVC_VEN_CST") %>' CssClass="control-label" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="廠商名稱">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOVC_VEN_TITLE" Text='<%# Bind("OVC_VEN_TITLE") %>' CssClass="control-label" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="廠商地址及電話">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOVC_VEN_ADDRESS" Text='<%# Bind("OVC_VEN_ADDRESS") %>' CssClass="control-label" runat="server"></asp:Label><br />
                                                                    <asp:Label ID="lblOVC_VEN_ITEL" Text='<%# Bind("OVC_VEN_TEL") %>' CssClass="control-label" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>

                                                    <table class="table table-bordered text-center">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnSaveVendor" CssClass="btn-default btnw2" Text="存檔" OnClick="btnSaveVendor_Click" runat="server" />
                                                                <asp:Button ID="btnReset" CssClass="btn-default btnw2" Text="清除" OnClick="btnReset_Click" runat="server"/><br />
                                                                <asp:Button ID="btnSaveAndInsert" CssClass="btn-default" Text="存檔並寫入廠商檔" OnClick="btnSaveAndInsert_Click" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtOVC_VEN_CST" CssClass="tb tb-s" runat="server"></asp:TextBox><br />
                                                                <asp:Button ID="btnVendorQuery" CssClass="btn-default" Text="廠商查詢" OnClick="btnVendorQuery_Click" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtOVC_VEN_TITLE" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server">地址:</asp:Label>
                                                                <asp:TextBox ID="txtOVC_VEN_ADDRESS" CssClass="tb tb-l" runat="server"></asp:TextBox><br />
                                                                <asp:Label CssClass="control-label" runat="server">電話:</asp:Label>
                                                                <asp:TextBox ID="txtOVC_VEN_TEL" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    
                                    
                                    
                                    
                                    <div id="TabCheck" class="tab-pane">
                                    <!-- 檢查項目明細 -->
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <div class="text-center diva">
                                                    <asp:LinkButton ID="lbtn" runat="server">廠商疑義異議</asp:LinkButton>
                                                </div>


                                                <table class="table table-bordered" style="text-align:center">
                                                    <tr>
                                                        <td style="width:5%" rowspan="22" ><asp:Label CssClass="control-label" runat="server">購案承辦人檢查項目</asp:Label></td>
                                                        <td style="width:5%"><asp:Label CssClass="control-label" runat="server" Text="項次"></asp:Label></td>
                                                        <td style="width:75%"><asp:Label CssClass="control-label" runat="server" Text="招標文件檢查項目表"></asp:Label></td>
                                                        <td style="width:25%"><asp:Label CssClass="control-label" runat="server" Text="是否"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM1" CssClass="control-label" runat="server" Text="1"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ1" CssClass="control-label" runat="server" Text="標單首頁及清單案號與原核定案號是否相符?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck1" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM2" CssClass="control-label" runat="server" Text="2"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ2" CssClass="control-label" runat="server" Text="等標期是否為法定招標期限標準天數以上"></asp:Label></td>
                                                        <td>
                                                             <asp:RadioButtonList ID="rdoCheck2" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM3" CssClass="control-label" runat="server" Text="3"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ3" CssClass="control-label" runat="server" Text="廠商請求釋放期限是否達等標期四分之一以上?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck3" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM4" CssClass="control-label" runat="server" Text="4"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ4" CssClass="control-label" runat="server" Text="首頁交貨日期、地點及付款辦法與清單是否相符?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck4" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM5" CssClass="control-label" runat="server" Text="5"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ5" CssClass="control-label" runat="server" Text="免繳押標金要求是否註明並公告?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck5" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM6" CssClass="control-label" runat="server" Text="6"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ6" CssClass="control-label" runat="server" Text="免繳履保金要求是否註明並公告?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck6" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM7" CssClass="control-label" runat="server" Text="7"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ7" CssClass="control-label" runat="server" Text="與投標須知不同之要求，標準首頁是否已特別註明?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck7" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM8" CssClass="control-label" runat="server" Text="8"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ8" CssClass="control-label" runat="server" Text="本中心投標須知是否併案檢討?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck8" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM9" CssClass="control-label" runat="server" Text="9"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ9" CssClass="control-label" runat="server" Text="本中心契約通用條款是否併案檢討?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck9" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server" >否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM10" CssClass="control-label" runat="server" Text="10"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ10" CssClass="control-label" runat="server" Text="投標、開標、殺價、決標方式是否明確述明?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck10" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM11" CssClass="control-label" runat="server" Text="11"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ11" CssClass="control-label" runat="server" Text="清單內容招標、投標特殊要求是否註明並公告?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck11" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM12" CssClass="control-label" runat="server" Text="12"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:DropDownList ID="drpQ12" CssClass="tb tb-s" OnSelectedIndexChanged="drpQ_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                                                <asp:ListItem Value="" Text="請選擇"></asp:ListItem>
                                                                <asp:ListItem>附件</asp:ListItem>
                                                                <asp:ListItem>附表</asp:ListItem>
                                                                <asp:ListItem>藍圖</asp:ListItem>
                                                                <asp:ListItem>草約</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtQ12" CssClass="tb tb-l" runat="server"></asp:TextBox><br>
                                                            <asp:Label ID="lblQ12" CssClass="control-label" runat="server" Text="頁次順序是否正確及納入?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck12" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM13" CssClass="control-label" runat="server" Text="13"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ13" CssClass="control-label" runat="server" Text="非公告預算購案清單內容、總價欄內金額是否已塗銷?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck13" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM14" CssClass="control-label" runat="server" Text="14"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:DropDownList ID="drpQ14" CssClass="tb tb-s" OnSelectedIndexChanged="drpQ_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                                                <asp:ListItem Value="" Text="請選擇"></asp:ListItem>
                                                                <asp:ListItem>清單</asp:ListItem>
                                                                <asp:ListItem>規格</asp:ListItem>
                                                                <asp:ListItem>藍圖</asp:ListItem>
                                                                <asp:ListItem>需求書</asp:ListItem>
                                                                <asp:ListItem>特別條款</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtQ14" CssClass="tb tb-l" runat="server"></asp:TextBox><br>
                                                            <asp:Label ID="lblQ14" CssClass="control-label" runat="server" Text="是否為最新版本(電子檔)?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck14" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM15" CssClass="control-label" runat="server" Text="15"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:DropDownList ID="drpQ15" OnSelectedIndexChanged="drpQ_SelectedIndexChanged" AutoPostBack="true" CssClass="tb tb-m" runat="server">
                                                                <asp:ListItem Value="" Text="請選擇"></asp:ListItem>
                                                                <asp:ListItem>不分段開標</asp:ListItem>
                                                                <asp:ListItem>一次投標分段開標</asp:ListItem>
                                                                <asp:ListItem>分段投標分段開標</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtQ15" CssClass="tb tb-l" runat="server"></asp:TextBox><br>
                                                            <asp:Label ID="lblQ15" CssClass="control-label" runat="server" Text="是否明確述明?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck15" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM16" CssClass="control-label" runat="server" Text="16"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ16" CssClass="control-label" runat="server" Text="選擇性招標購案之招標文件中是否註明押標金應檢附於資格審查後之下一階段?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck16" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM17" CssClass="control-label" runat="server" Text="17"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ17" CssClass="control-label" runat="server" Text="具後續擴充需要之購案，其項目、內容、金額、數量或期間上限是否於招標公告及招標文件內敘明?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck17" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM18" CssClass="control-label" runat="server" Text="18"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ18" CssClass="control-label" runat="server" Text="是否核定免用本中心契約通用條款及投標須知?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck18" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM19" CssClass="control-label" runat="server" Text="19"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ19" CssClass="control-label" runat="server" Text="購案屬分段開標、最有利標、重大複雜及需現場履勘等相關特殊要求，預判需耗時甚長始可辦理價格標，是否應特別註明延長廠商殺價有效期?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck19" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM20" CssClass="control-label" runat="server" Text="20"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ20" CssClass="control-label" runat="server" Text="經驗討具前(19)條情形，投標廠商以銀行、保險公司速帶保證書或不可撤銷保信用狀代替押標金繳交者，是否應特別註明一併延長其有效期?"></asp:Label></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck20" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblONB_ITEM21" CssClass="control-label" runat="server" Text="21"></asp:Label></td>
                                                        <td class="text-left">
                                                            <asp:Label ID="lblQ21" CssClass="control-label" runat="server" Text="其他:">
                                                            </asp:Label><asp:TextBox ID="txtOthers" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdoCheck21" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                <asp:ListItem class="rdoOVC_RESULT_Item1" runat="server">是</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item2" runat="server">否</asp:ListItem>
                                                                <asp:ListItem class="rdoOVC_RESULT_Item3" runat="server">免審</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table>


                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--頁籤－結束-->
                        <table class="table table-bordered">
                            <tr>
                                <td style="width:50%;border-right:none" class="text-right" >
                                    <asp:Label CssClass="control-label text-red" runat="server">主官核批日：</asp:Label></td>
                                <td style="border-left: none">
                                    <div class="input-append datepicker position-left" style="border-left-style:none;border-left:none">
                                        <asp:TextBox ID="txtOVC_DAPPROVE" CssClass="tb tb-s position-left"  runat="server" ></asp:TextBox>
                                        <div class="add-on"><i class="icon-calendar"></i></div>
                                    </div>
                                </td>
                            </tr>
                        </table><br />

                        <div style="text-align:center">
                            <asp:Button ID="btnSave" Text="存檔" OnClick="btnSave_Click" CssClass="btn-default btnw2" runat="server" />
                            <asp:Button ID="btnReturn" Text="回開標通知選擇畫面" OnClick="btnReturn_Click" CssClass="btn-default" runat="server" />
                            <asp:Button ID="btnReturnM" Text="回主流程畫面" OnClick="btnReturnM_Click" CssClass="btn-default btnw6" runat="server" />
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

