<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D22.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D22" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                    契約製作檢查項目編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" id="divForm" style="border: solid 2px;" visible="true" runat="server">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <table class="table no-bordered-seesaw text-center">
                                <tr class="no-bordered-seesaw">
                                    <td class="no-bordered">
                                        <asp:Label CssClass="control-label" runat="server" Text="購案契約編號：" Font-Size="Large"></asp:Label>
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server" Font-Size="Large"></asp:Label>
                                    </td>
                                    <td class="no-bordered">
                                        <asp:Label CssClass="control-label" runat="server" Text="合約商：" Font-Size="Large"></asp:Label>
                                        <asp:Label ID="lblOVC_VEN_TITLE" CssClass="control-label" runat="server" Font-Size="Large"></asp:Label>
                                    </td>
                                    <td class="no-bordered">
                                        <asp:Label CssClass="control-label" runat="server" Text="組別：" Font-Size="Large"></asp:Label>
                                        <asp:Label ID="lblONB_GROUP" CssClass="control-label" runat="server" Font-Size="Large"></asp:Label><br>
                                    </td>
                                </tr>
                            </table>
                            <br>
                            <br>
                            <table class="table table-bordered" style="text-align: center">
                                <tr>
                                    <td style="width: 10%">
                                        <asp:Label CssClass="control-label" runat="server" Text="項次"></asp:Label></td>
                                    <td style="width: 60%">
                                        <asp:Label CssClass="control-label" runat="server" Text="契約製作檢查項目表"></asp:Label></td>
                                    <td style="width: 20%">
                                        <asp:Label CssClass="control-label" runat="server" Text="是否"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFirst" CssClass="control-label" runat="server" Text="1"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblQ1" CssClass="control-label" runat="server" Text="封面、首頁及契約製作內相關附件購案號是否正確?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck1" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList><asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSecond" CssClass="control-label" runat="server" Text="2"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblQ2" CssClass="control-label" runat="server" Text="編排順序、頁次及內容是否與契約目錄相符?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck2" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList><asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblThird" CssClass="control-label" runat="server" Text="3"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblQ3" CssClass="control-label" runat="server" Text="清單、決標紀錄之單、總價及品名是否相符?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck3" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFourth" CssClass="control-label" runat="server" Text="4"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:CheckBoxList ID="cblQ4" CssClass="radioButton" OnSelectedIndexChanged="cblQ4_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem>附件</asp:ListItem>
                                            <asp:ListItem>附表</asp:ListItem>
                                            <asp:ListItem>藍圖</asp:ListItem>
                                            <asp:ListItem>樣品</asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:DropDownList Visible="false" ID="drpQ4" AutoPostBack="true" OnSelectedIndexChanged="drpQ4_SelectedIndexChanged" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>請選擇</asp:ListItem>
                                            <asp:ListItem>附件</asp:ListItem>
                                            <asp:ListItem>附表</asp:ListItem>
                                            <asp:ListItem>藍圖</asp:ListItem>
                                            <asp:ListItem>樣品</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtQ4" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                        <asp:Label ID="lblQ4" CssClass="control-label" runat="server" Text="是否齊全納入契約?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck4" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFifth" CssClass="control-label" runat="server" Text="5"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblQ5" CssClass="control-label" runat="server" Text="契約內容、文字及圖表是否清晰可見?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck5" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSixth" CssClass="control-label" runat="server" Text="6"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblQ6" CssClass="control-label" runat="server" Text="廠商投標文件是否要求納入契約一併執行?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck6" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSeventh" CssClass="control-label" runat="server" Text="7"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblQ7" CssClass="control-label" runat="server" Text="首頁、清單及決標紀錄大、小寫金額是否正確?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck7" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblEighth" CssClass="control-label" runat="server" Text="8"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblQ8" CssClass="control-label" runat="server" Text="契約內塗改處是否有蓋校正章?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck8" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNinth" CssClass="control-label" runat="server" Text="9"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblQ9" CssClass="control-label" runat="server" Text="本部投標須知是否併案納入?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck9" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTenth" CssClass="control-label" runat="server" Text="10"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblQ10" CssClass="control-label" runat="server" Text="本部契約通用條款是否併案納入?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck10" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblEleventh" CssClass="control-label" runat="server" Text="11"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblQ11" CssClass="control-label" runat="server" Text="同等品相關文件或規格建議書等是否已納入契約?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck11" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTwelfth" CssClass="control-label" runat="server" Text="12"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblQ12" CssClass="control-label" runat="server" Text="首頁交貨地點、交貨期限及付款方式與清單要求是否相符?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck12" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblThirteenth" CssClass="control-label" runat="server" Text="13"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:CheckBoxList ID="cblQ13" CssClass="radioButton" OnSelectedIndexChanged="cblQ13_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem>免營業稅</asp:ListItem>
                                            <asp:ListItem>免貨物稅</asp:ListItem>
                                            <asp:ListItem>免進口稅</asp:ListItem>
                                            <asp:ListItem>含稅</asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:DropDownList Visible="false" ID="drpQ13" AutoPostBack="true" OnSelectedIndexChanged="drpQ13_SelectedIndexChanged" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>請選擇</asp:ListItem>
                                            <asp:ListItem>免營業稅</asp:ListItem>
                                            <asp:ListItem>免貨物稅</asp:ListItem>
                                            <asp:ListItem>免進口稅</asp:ListItem>
                                            <asp:ListItem>含稅</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtQ13" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                        <asp:Label ID="lblQ13" CssClass="control-label" runat="server" Text="記述是否與原核定需求相符?"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck13" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFourteenth" CssClass="control-label" runat="server" Text="14"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblQ14" CssClass="control-label" runat="server" Text="清單內容要求簽約前應完成事項是否完成?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck14" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFifteenth" CssClass="control-label" runat="server" Text="15"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblQ15" CssClass="control-label" runat="server" Text="履保金繳交金額是否足額符合標準規定?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck15" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSixteenth" CssClass="control-label" runat="server" Text="16"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblQ16" CssClass="control-label" runat="server" Text="得標廠商以銀行、保險公司連帶保證書或不可撤銷擔保信用狀代替履保金繳交者，其有效期是否叫最後交貨或安裝期限延長達九十日以上?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck16" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSeventeenth" CssClass="control-label" runat="server" Text="17"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblQ17" CssClass="control-label" runat="server" Text="得標廠商以銀行、保險公司連帶保證書或不可撤銷擔保信用狀等代替履保金繳交是否完成對保查證作業?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck17" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblEighteenth" CssClass="control-label" runat="server" Text="18"></asp:Label></td>
                                    <td class="text-left">
                                        <asp:Label ID="lblQ18" CssClass="control-label" runat="server" Text="契約內容所用版本是否為最新電子檔?"></asp:Label></td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoCheck18" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>是</asp:ListItem>
                                            <asp:ListItem>否</asp:ListItem>
                                            <asp:ListItem>免審</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server" Text="綜辦"></asp:Label><br>
                                        <br>
                                        <asp:Label CssClass="control-label" runat="server" Text="意見"></asp:Label>
                                    </td>
                                    <td colspan="3" class="text-left">
                                        <asp:RadioButtonList ID="rdoOVC_DO_NAME_RESULT" CssClass="radioButton" runat="server" RepeatLayout="UnorderedList">
                                            <asp:ListItem>簽約前審查正本文件及公司大、小章等與投標文件相符後簽約。</asp:ListItem>
                                            <asp:ListItem>與得標廠商履保金連帶保證書或擔保信用狀完成對保，並於簽約前審查廠商正本文件及公司大、小章等與投標文件相符後簽約。</asp:ListItem>
                                            <asp:ListItem>洽委購單位澄清後辦理。</asp:ListItem>
                                            <asp:ListItem>洽得標廠商澄清後辦理。</asp:ListItem>
                                            <asp:ListItem>其他</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:TextBox ID="TextBox1" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td style="width: 50%" class="text-right">
                                        <asp:Label CssClass="control-label" runat="server">主官核批日：</asp:Label></td>
                                    <td>
                                        <div class="input-append datepicker">
                                            <asp:TextBox ID="txtOVC_DAPPROVE" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center">
                                <asp:Button ID="btnSave" OnClick="btnSave_Click" CssClass="btn-warning btnw2" runat="server" Text="存檔" />
                                <asp:Button ID="btnBack" CssClass="btn-warning btnw4" OnClick="btnBack_Click" runat="server" Text="回上一頁" />
                                <asp:LinkButton ID="lbtnCheck" OnClick="lbtnCheck_Click" runat="server">列印契約檢查項目表.doc</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lbtnCheck_odt" OnClick="lbtnCheck_odt_Click" runat="server">列印契約檢查項目表.odt</asp:LinkButton>
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
