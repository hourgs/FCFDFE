<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D30.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D30" %>

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
                <header class="title text-blue">
                    <!--標題-->
                    廠商投標文件檢查表編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <table class="table table-bordered text-center">
                            <tr>
                                <td style="width: 15%">
                                    <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td style="width: 15%">
                                    <asp:Label CssClass="control-label" runat="server">開標時間</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_DOPEN" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">投標標的</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_BID_LANGUAGE" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">廠商名稱</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_VEN_TITLE" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div class="text-center" style="margin-bottom: 10px;">
                            <asp:Button ID="btnCopy" CssClass="btn-warning btnw6" runat="server" Text="複製審查資料" />
                        </div>


                        <table class="table table-bordered text-left">
                            <tr>
                                <th style="width: 25%">
                                    <asp:Label CssClass="control-label" runat="server">審查項目</asp:Label></th>
                                <th style="width: 10%">
                                    <asp:Label CssClass="control-label text-red" runat="server">合格</asp:Label></th>
                                <th style="width: 65%" colspan="2">
                                    <asp:Label ID="Label3" CssClass="control-label" runat="server">備註</asp:Label></th>
                            </tr>
                            <tr>
                                <td rowspan="6" class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server">1. 廠商資格</asp:Label>
                                </td>
                                <td rowspan="3">
                                    <asp:DropDownList ID="drpOVC_VENDOR_DESC" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem Value="N">免審</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:CheckBoxList ID="chkOVC_VENDOR_DESC" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem Value="Company">公司執照</asp:ListItem>
                                        <asp:ListItem Value="Business_R">營利事業登記證</asp:ListItem>
                                        <asp:ListItem Value="Attorney">原廠授權書</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBoxList ID="chkother" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem Value="other">其他</asp:ListItem>
                                    </asp:CheckBoxList><asp:TextBox ID="txtother" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label CssClass="control-label text-red" runat="server">不合格原因</asp:Label>
                                    <asp:TextBox ID="txtOVC_VENDOR_DESC_NO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td rowspan="3">
                                    <asp:DropDownList ID="drpOVC_VENDOR_DESC2" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem Value="N">免審</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="chkOVC_VENDOR_DESC2" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem Value="Financial">財力證明</asp:ListItem>
                                        <asp:ListItem Value="Credit">信用證明</asp:ListItem>
                                        <asp:ListItem Value="Exp_Per">經驗或實績證明</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                                <td rowspan="3">
                                    <asp:Label CssClass="control-label text-red" runat="server">本項由委購單位審查及簽章</asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="chkother2" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem Value="other">其他</asp:ListItem>
                                    </asp:CheckBoxList><asp:TextBox ID="txtother2" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label text-red" runat="server">不合格原因</asp:Label>
                                    <asp:TextBox ID="txtOVC_VENDOR_DESC_NO2" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td rowspan="3" class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server">2. 繳稅證明</asp:Label>
                                </td>
                                <td rowspan="3">
                                    <asp:DropDownList ID="drpTax_c" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem Value="N">免審</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:CheckBoxList ID="chkTax_c" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem Value="Business_Tax">最近一期「營業繳稅證書」</asp:ListItem>
                                        <asp:ListItem Value="Sales_Tax">主管稽徵機關核章之最近一期「營業人銷售額與稅額申報書」收執聯</asp:ListItem>
                                        <asp:ListItem Value="Non-re">非拒絕來戶證明</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBoxList ID="chkTax_other" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem Value="N">其他</asp:ListItem>
                                    </asp:CheckBoxList><asp:TextBox ID="txtTax_other" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label CssClass="control-label text-red" runat="server">不合格原因</asp:Label>
                                    <asp:TextBox ID="txtTax_other_NO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server">3. 投標廠商聲明書</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpOnb_bid" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem Value="N">免審</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:Label CssClass="control-label text-red" runat="server">不合格原因</asp:Label>
                                    <asp:TextBox ID="txtOnb_bid" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server">4.<asp:CheckBoxList ID="chkOVC_CHECK_RESULT" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                        <asp:ListItem Value="File">規格文件</asp:ListItem>
                                        <asp:ListItem Value="Same_product">同等品審查</asp:ListItem>
                                        <asp:ListItem Value="Proposals">建議書</asp:ListItem>
                                    </asp:CheckBoxList></asp:Label>
                                    <asp:TextBox ID="txtOVC_CHECK_RESULT_NUM" CssClass="tb tb-xs" runat="server"></asp:TextBox>份

                                </td>
                                <td>
                                    <asp:DropDownList ID="drpOVC_CHECK_RESULT" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem Value="N">免審</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label text-red" runat="server">不合格原因</asp:Label>
                                    <asp:TextBox ID="txtOVC_CHECK_RESULT_NO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label text-red" runat="server">本項由委購單位審查及簽章</asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server">5. 投標報價單(附件清單)單、總價</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpOVC_CHECK_DOC" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem Value="N">免審</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:Label CssClass="control-label text-red" runat="server">不合格原因</asp:Label>
                                    <asp:TextBox ID="txtOVC_CHECK_DOC_NO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server">6. 投標報價單內 廠商簽章</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpTenderSign" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem Value="值">免審</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:Label CssClass="control-label text-red" runat="server">不合格原因</asp:Label>
                                    <asp:TextBox ID="txtTenderSign_NO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server">7. 清單內容</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drplistitem" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem Value="N">免審</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label text-red" runat="server">不合格原因</asp:Label>
                                    <asp:TextBox ID="txtlistitem_NO" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                </td>

                                <td>
                                    <asp:Label CssClass="control-label text-red" runat="server">本項由委購單位審查及簽章</asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td rowspan="1" class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server">8. 押標金單據<br />(本案押標金不少於各組投標文件標價3%。)</asp:Label>
                                </td>
                                <td rowspan="2">
                                    <asp:DropDownList ID="drpTENDER_LC" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem Value="N">免審</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td rowspan="2" colspan="2">
                                    <asp:CheckBoxList ID="chkTENDER_LC" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                        <asp:ListItem Value="BaCh">銀行支票/本票</asp:ListItem>
                                        <asp:ListItem Value="PosOrd">郵政匯票</asp:ListItem>
                                        <asp:ListItem Value="NaArmy_2">國軍財務單位押標金收據第二聯</asp:ListItem>
                                        <asp:ListItem Value="BaConfiofIrrGuCredit">銀行開發或保兌之不可撤銷擔保信用狀正本</asp:ListItem>
                                        <asp:ListItem Value="BaWriJoGuarantee">銀行開發之書面連帶保證正本</asp:ListItem>
                                        <asp:ListItem Value="InsurIrr">保險公司開發之連帶保證保險單正本</asp:ListItem>
                                    </asp:CheckBoxList>

                                    <br />
                                    <asp:Label CssClass="control-label" runat="server">其他：</asp:Label>
                                    <asp:TextBox ID="txtTENDER_LC_other" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    <br />
                                    <asp:Label CssClass="control-label text-red" runat="server">不合格原因：</asp:Label>
                                    <asp:CheckBoxList ID="chkTENDER_LC_NO" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem Value="MoneyNM">金額不符</asp:ListItem>
                                        <asp:ListItem Value="BeNM">受款/益或被保證/險人不符</asp:ListItem>
                                        <asp:ListItem Value="CheIssNM">支票簽發人不符</asp:ListItem>
                                    </asp:CheckBoxList>
                                    <br />

                                    <asp:Label CssClass="control-label" runat="server">(投標廠商以上述打V者繳交，其有效期為符合招標文件規定)</asp:Label>
                                    <br />
                                    <asp:Label CssClass="control-label" runat="server">其他：</asp:Label>
                                    <asp:TextBox ID="txtTENDER_LC_other2" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label text-red" runat="server">金額：</asp:Label>
                                    <asp:TextBox ID="txtMoney" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server">9.其他</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpMonet" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem Value="N">免審</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:Label CssClass="control-label text-red" runat="server">自行輸入：</asp:Label>
                                    <asp:TextBox ID="txtOtherinput" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-center">
                                    <asp:Label CssClass="control-label text-red" runat="server">10.拒絕往來廠商</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpYN" CssClass="tb tb-s" runat="server">
                                        <asp:ListItem Value="N">否</asp:ListItem>
                                        <asp:ListItem Value="Y">是</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:Label CssClass="control-label text-red" runat="server">(本項為是者，即為不合格標)</asp:Label>
                                </td>
                            </tr>
                        </table>


                        <table class="table table-bordered text-center">
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label text-red" runat="server">審查結果</asp:Label>
                                    <asp:RadioButtonList ID="rdoOVC_RESULT" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                       <asp:ListItem Value="Y">合格</asp:ListItem>
                                        <asp:ListItem Value="N">不合格</asp:ListItem>
                                        </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                        <div class="text-center">
                            <asp:Button ID="btnSave" CssClass="btn-warning btnw2" runat="server" Text="存檔" />
                            <asp:Button ID="btnReturn" CssClass="btn-warning " runat="server" Text="回截標審查作業畫面" />
                            <asp:Button ID="btnReturnM" CssClass="btn-warning btnw6" runat="server" Text="回主流程畫面" />
                            <a href="#" style="text-decoration:underline; color:red; font-size:18px;">廠商投標文件審查表預覽列印</a>
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
