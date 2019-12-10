<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D13.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D13" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .rdoOVC_RESULT_Item1 input[type=radio]:checked + label {color:blue;}
        .rdoOVC_RESULT_Item2 input[type=radio]:checked + label {color:red;}
        .rdoOVC_RESULT_Item3 input[type=radio]:checked + label {color:green;}
    </style>

    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->
                    計畫清單移轉檢查項目表編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" id="divForm" style=" border: solid 2px;" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="Width:40%"><asp:Label CssClass="control-label" Text="購案編號" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:Label ID="lblOVC_PURCH_A" CssClass="control-label" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtOVC_PURCH_5" CssClass="tb tb-s" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;(採購號碼三碼)
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" Text="購案名稱" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" Text="申購單位(代碼)-申購人(電話)" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:Label ID="lblOVC_PUR_NSECTION" CssClass="control-label" runat="server"></asp:Label>(
                                        <asp:Label ID="lblOVC_PUR_SECTION" CssClass="control-label" runat="server"></asp:Label>) -
                                        <asp:Label ID="lblOVC_PUR_USER" CssClass="control-label" runat="server"></asp:Label> (
                                        <asp:Label ID="lblOVC_PUR_IUSER_PHONE" CssClass="control-label" runat="server"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label CssClass="control-label" runat="server">軍線：</asp:Label>
                                        <asp:Label ID="lblOVC_PUR_IUSER_PHONE_EXT" CssClass="control-label" runat="server"></asp:Label>
                                        <asp:Label CssClass="control-label" runat="server">)</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" Text="採購屬性" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:Label ID="lblOVC_LAB" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" Text="招標方式" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:Label ID="lblOVC_PUR_ASS_VEN_CODE" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" Text="投標段次" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:Label ID="lblOVC_BID_TIMES" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" Text="決標原則" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:Label ID="lblOVC_BID" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" Text="核評移案日" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%"> 
                                        <asp:Label ID="lblSTATUS" CssClass="control-label" Style="display:none" runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_DBEGIN" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label ID="lblOVC_DO_NAME" CssClass="control-label" Text="指派承辦人" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <asp:DropDownList ID="drpOVC_DO_NAME" CssClass="tb tb-l" OnSelectedIndexChanged="drpOVC_DO_NAME_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        <asp:TextBox ID="txtOVC_DO_NAME" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                        <asp:Label ID="lblChangeOVC_DO_NAME" Style="display:none" runat="server"></asp:Label>
                                        <asp:Label ID="lblOVC_FROM_NAME" style="display:none" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" Text="收辦日" runat="server"></asp:Label></td>
                                    <td class="text-left" style="Width:60%">
                                        <div class="input-append datepicker position-left">
                                            <asp:TextBox ID="txtOVC_DRECEIVE" CssClass="tb tb-s position-left text-change" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            

                            <table class="table table-bordered text-center">
                                <tr>
                                    <td rowspan="5" style="Width:5%" class="td-vertical"><asp:Label CssClass="control-label text-vertical-m" style="height: 280px;" runat="server">登管人員檢查項目</asp:Label></td>
                                    <td style="Width:10%"><asp:Label CssClass="control-label" runat="server"  style="Width:50px" Text="項次"></asp:Label></td>
                                    <td style="Width:60%"><asp:Label CssClass="control-label" runat="server" Text="登管人員檢查項目"></asp:Label></td>
                                    <td style="Width:25%"><asp:Label CssClass="control-label" runat="server" Text="是否"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td  style="Width:10%">
                                        <asp:Label ID="lblONB_ITEM_1" CssClass="control-label text-red" runat="server">1</asp:Label></td>
                                    <td class="text-left"  style="Width:60%">
                                        <asp:Label ID="lblOVC_ITEM_NAME_1" CssClass="control-label text-red" runat="server">核對購案號是否正確?</asp:Label></td>
                                    <td  style="Width:25%">
                                        <asp:RadioButtonList ID="rdoOVC_RESULT_1" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label ID="lblONB_ITEM_2" CssClass="control-label text-red" runat="server">2</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_ITEM_NAME_2" CssClass="control-label text-red" runat="server">核定書及清單、品名、金額是否相符?</asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoOVC_RESULT_2" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblONB_ITEM_3" CssClass="control-label text-red" runat="server" Text="3"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblOVC_ITEM_NAME_3" CssClass="control-label text-red" runat="server" Text="是否已登入本中心購案時程管制資訊系統?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoOVC_RESULT_3" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblONB_ITEM_4" CssClass="control-label text-red" runat="server">4</asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label CssClass="control-label text-red" runat="server">其他：</asp:Label>
                                        <asp:TextBox ID="txtOVC_ITEM_NAME_4" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoOVC_RESULT_4" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                </tr>
                            </table>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-red" runat="server" Text="綜辦"></asp:Label><br>
                                        <asp:Label CssClass="control-label text-red" runat="server" Text="意見"></asp:Label></td>
                                    <td colspan="5" class="text-left">
                                        <asp:RadioButtonList ID="rdoOVC_NAME_RESULT" CssClass="radioButton" runat="server" RepeatLayout="UnorderedList"></asp:RadioButtonList></td>
                                </tr>
                            </table>

                                              
                            <table class="table table-bordered">
                                <tr>
                                    <td style="width:50%;border-right:none" class="text-right">
                                        <asp:Label CssClass="control-label text-red" runat="server">完成審查日：</asp:Label></td>
                                    <td style="border-left: none;">
                                        <div class="input-append datepicker position-left" style="border-left-style:none;border-left:none"">
                                            <asp:TextBox ID="txtOVC_DAPPROVE" CssClass="tb tb-s position-left text-change" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div></td>
                                </tr>
                            </table>
                            
                            <div style="text-align:center"> 
                                <asp:Button ID="btnSave" CssClass="btn-default btnw2" Text="存檔" OnClick="btnSave_Click" runat="server" />&nbsp;
                                <asp:Button ID="btnBack" CssClass="btn-default" OnClick="btnBack_Click" Text="回上一頁" runat="server"/>
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
