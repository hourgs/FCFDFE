<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D18_4.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D18_4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    廠商投標文件檢查表編輯
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;" id="divForm" visible="false" runat="server">
                    <div class="form" style="border: 5px;">
                        <table class="table table-bordered text-center">
                            <tr>
                                <td style="width: 15%">
                                    <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_PURCH_A_5" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_PURCH" Style="display:none" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="lblOVC_PURCH_5" Style="display:none"  CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td style="width: 15%">
                                    <asp:Label CssClass="control-label" runat="server">開標時間</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_DOPEN" runat="server"></asp:Label>
                                    <asp:Label CssClass="control-label" runat="server">　</asp:Label>
                                    <asp:Label ID="lblOVC_OPEN_HOUR" runat="server"></asp:Label>
                                    <asp:Label CssClass="control-label" runat="server">時</asp:Label>
                                    <asp:Label ID="lblOVC_OPEN_MIN" runat="server"></asp:Label>
                                    <asp:Label CssClass="control-label" runat="server">分　(第</asp:Label>
                                    <asp:Label ID="lblONB_TIMES" CssClass="control-label text-red" runat="server"></asp:Label>
                                    <asp:Label CssClass="control-label" runat="server">次)　</asp:Label>
                                    <asp:Label CssClass="control-label text-red" runat="server">組別：</asp:Label>
                                    <asp:Label ID="lblONB_GROUP" CssClass="control-label text-red" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">投標標的</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">廠商名稱</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOVC_VEN_TITLE" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>

                        
                        <div id="divTBM1313_ITEM" runat="server">
                            <div class="text-center" style="margin-bottom: 10px;">
                                <asp:Button ID="btnCopy" Text="複製審查資料" OnClick="btnCopy_Click" CssClass="btn-default btnw6" runat="server" />
                            </div>

                            <table class="table table-bordered text-left">
                                <tr>
                                    <th style="width: 25%">
                                        <asp:Label CssClass="control-label" runat="server">審查項目</asp:Label></th>
                                    <th style="width: 10%">
                                        <asp:Label CssClass="control-label text-red" runat="server">合格</asp:Label></th>
                                    <th style="width: 65%" colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">備註</asp:Label></th>
                                </tr>
                                <tr>
                                    <td rowspan="6" class="text-center">
                                        <asp:Label CssClass="control-label text-red" runat="server">1. 廠商資格</asp:Label>
                                    </td>
                                    <td rowspan="3">
                                        <asp:DropDownList ID="drpOVC_RESULT_10" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="">請選擇</asp:ListItem>
                                            <asp:ListItem Value="Y">合格</asp:ListItem>
                                            <asp:ListItem Value="N">不合格</asp:ListItem>
                                            <asp:ListItem Value="X">免審</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:CheckBoxList ID="chkOVC_ITEM_NAME_10" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem>公司執照</asp:ListItem>
                                            <asp:ListItem>營利事業登記證</asp:ListItem>
                                            <asp:ListItem>原廠授權書</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">其他：</asp:Label>
                                        <asp:TextBox ID="txtOther_10" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-red" runat="server">不合格原因</asp:Label>
                                        <asp:TextBox ID="txtREJECT_10" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="3">
                                        <asp:DropDownList ID="drpOVC_RESULT_11" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="">請選擇</asp:ListItem>
                                            <asp:ListItem Value="Y">合格</asp:ListItem>
                                            <asp:ListItem Value="N">不合格</asp:ListItem>
                                            <asp:ListItem Value="X">免審</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:CheckBoxList ID="chkOVC_ITEM_NAME_11" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
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
                                        <asp:Label CssClass="control-label" runat="server">其他：</asp:Label>
                                        <asp:TextBox ID="txtOther_11" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label text-red" runat="server">不合格原因</asp:Label>
                                        <asp:TextBox ID="txtREJECT_11" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="3" class="text-center">
                                        <asp:Label CssClass="control-label text-red" runat="server">2. 繳稅證明</asp:Label>
                                    </td>
                                    <td rowspan="3">
                                        <asp:DropDownList ID="drpOVC_RESULT_20" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="">請選擇</asp:ListItem>
                                            <asp:ListItem Value="Y">合格</asp:ListItem>
                                            <asp:ListItem Value="N">不合格</asp:ListItem>
                                            <asp:ListItem Value="X">免審</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:CheckBoxList ID="chkOVC_ITEM_NAME_20" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem>最近一期「營業稅繳款書」</asp:ListItem>
                                            <asp:ListItem>主管稽徵機關核章之最近一期「營業人銷售額與稅額申報書」收執聯</asp:ListItem>
                                            <asp:ListItem>非拒絕來戶證明</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label" runat="server">其他：</asp:Label>
                                        <asp:TextBox ID="txtOther_20" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-red" runat="server">不合格原因</asp:Label>
                                        <asp:TextBox ID="txtREJECT_20" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label text-red" runat="server">3. 投標廠商聲明書</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_RESULT_30" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="">請選擇</asp:ListItem>
                                            <asp:ListItem Value="Y">合格</asp:ListItem>
                                            <asp:ListItem Value="N">不合格</asp:ListItem>
                                            <asp:ListItem Value="X">免審</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-red" runat="server">不合格原因</asp:Label>
                                        <asp:TextBox ID="txtREJECT_30" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label text-red" runat="server">4.
                                            <asp:CheckBoxList ID="chkOVC_ITEM_NAME_40" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                                <asp:ListItem>規格文件</asp:ListItem>
                                                <asp:ListItem>同等品審查</asp:ListItem>
                                                <asp:ListItem>建議書</asp:ListItem>
                                            </asp:CheckBoxList></asp:Label>
                                        <asp:TextBox ID="txtOVC_ITEM_NAME_40" CssClass="tb tb-xs" runat="server"></asp:TextBox>份
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_RESULT_40" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="">請選擇</asp:ListItem>
                                            <asp:ListItem Value="Y">合格</asp:ListItem>
                                            <asp:ListItem Value="N">不合格</asp:ListItem>
                                            <asp:ListItem Value="X">免審</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-red" runat="server">不合格原因</asp:Label>
                                        <asp:TextBox ID="txtREJECT_40" CssClass="tb tb-l" runat="server"></asp:TextBox>
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
                                        <asp:DropDownList ID="drpOVC_RESULT_50" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="">請選擇</asp:ListItem>
                                            <asp:ListItem Value="Y">合格</asp:ListItem>
                                            <asp:ListItem Value="N">不合格</asp:ListItem>
                                            <asp:ListItem Value="X">免審</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-red" runat="server">不合格原因</asp:Label>
                                        <asp:TextBox ID="txtREJECT_50" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label text-red" runat="server">6. 投標報價單內 廠商簽章</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_RESULT_60" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="">請選擇</asp:ListItem>
                                            <asp:ListItem Value="Y">合格</asp:ListItem>
                                            <asp:ListItem Value="N">不合格</asp:ListItem>
                                            <asp:ListItem Value="X">免審</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-red" runat="server">不合格原因</asp:Label>
                                        <asp:TextBox ID="txtREJECT_60" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label text-red" runat="server">7. 清單內容</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_RESULT_70" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="">請選擇</asp:ListItem>
                                            <asp:ListItem Value="Y">合格</asp:ListItem>
                                            <asp:ListItem Value="N">不合格</asp:ListItem>
                                            <asp:ListItem Value="X">免審</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label CssClass="control-label text-red" runat="server">不合格原因</asp:Label>
                                        <asp:TextBox ID="txtREJECT_70" CssClass="tb tb-l" runat="server"></asp:TextBox>
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
                                        <asp:DropDownList ID="drpOVC_RESULT_80" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="">請選擇</asp:ListItem>
                                            <asp:ListItem Value="Y">合格</asp:ListItem>
                                            <asp:ListItem Value="N">不合格</asp:ListItem>
                                            <asp:ListItem Value="X">免審</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td rowspan="2" colspan="2">
                                        <asp:CheckBoxList ID="chkOVC_ITEM_NAME_80" CssClass="radioButton" RepeatLayout="UnorderedList" runat="server">
                                            <asp:ListItem>銀行支票/本票</asp:ListItem>
                                            <asp:ListItem>郵政匯票</asp:ListItem>
                                            <asp:ListItem>國軍財務單位押標金收據第二聯</asp:ListItem>
                                            <asp:ListItem>銀行開發或保兌之不可撤銷擔保信用狀正本</asp:ListItem>
                                            <asp:ListItem>銀行開發之書面連帶保證正本</asp:ListItem>
                                            <asp:ListItem>保險公司開發之連帶保證保險單正本</asp:ListItem>
                                        </asp:CheckBoxList><br />

                                        <asp:Label CssClass="control-label" runat="server">其他：</asp:Label>
                                        <asp:TextBox ID="txtOther_80" CssClass="tb tb-l" runat="server"></asp:TextBox><br />

                                        <asp:Label CssClass="control-label text-red" runat="server">不合格原因：</asp:Label>
                                        <asp:CheckBoxList ID="chkREJECT_80" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem>金額不符</asp:ListItem>
                                            <asp:ListItem>受款/益或被保證/險人不符</asp:ListItem>
                                            <asp:ListItem>支票簽發人不符</asp:ListItem>
                                        </asp:CheckBoxList>
                                        <br />

                                        <asp:Label CssClass="control-label" runat="server">(投標廠商以上述打V者繳交，其有效期為符合招標文件規定)</asp:Label>
                                        <br />
                                        <asp:Label CssClass="control-label" runat="server">其他：</asp:Label>
                                        <asp:TextBox ID="txtREJECT_80" CssClass="tb tb-l" runat="server"></asp:TextBox>
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
                                        <asp:DropDownList ID="drpOVC_RESULT_90" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="">請選擇</asp:ListItem>
                                            <asp:ListItem Value="Y">合格</asp:ListItem>
                                            <asp:ListItem Value="N">不合格</asp:ListItem>
                                            <asp:ListItem Value="X">免審</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label CssClass="control-label text-red" runat="server">自行輸入：</asp:Label>
                                        <asp:TextBox ID="txtOther_90" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <asp:Label CssClass="control-label text-red" runat="server">10.拒絕往來廠商</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpOVC_RESULT_100" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem Value="">請選擇</asp:ListItem>
                                            <asp:ListItem Value="Y">是</asp:ListItem>
                                            <asp:ListItem Value="N">否</asp:ListItem>
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
                                        <asp:RadioButtonList ID="rdoOVC_RESULT" CssClass="radioButton text-red" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="Y">合格</asp:ListItem>
                                            <asp:ListItem Value="N">不合格</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        
                            <div class="text-center">
                                <asp:Button ID="btnSave" Text="存檔" OnClick="btnSave_Click" CssClass="btn-default btnw2" runat="server" />
                                <asp:Button ID="btnSave_OK" OnClick="btnSave_OK_Click" Style="display:none" runat="server" />
                                <asp:Button ID="btnReturn" Text="回廠商編輯" OnClick="btnReturn_Click" CssClass="btn-default" runat="server" />
                                <asp:Button ID="btnReturnR" Text="回開標紀錄選擇畫面" CssClass="btn-default" OnClick="btnReturnR_Click" runat="server" />
                                <asp:Button ID="btnReturnM" Text="回主流程畫面" OnClick="btnReturnM_Click" CssClass="btn-default btnw6" runat="server" /><br />
                            </div>
                        </div>




                        <div id="divTBM1313" visible="false" runat="server">
                            <asp:GridView ID="gvTBM1313" CssClass="table data-table table-striped border-top text-center" OnRowCommand="gvTBM1313_RowCommand" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderText="請選擇">
                                        <ItemTemplate>
                                            <asp:Button ID="btnCopy" Text="複製" CommandName="Copy" CssClass="btn-default btnw2" runat="server" />
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
                                    <asp:BoundField HeaderText="廠商電話" DataField="OVC_VEN_TEL" />
                                    <asp:TemplateField HeaderText="開標日期">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOVC_DBID" Text='<%# Bind("OVC_DBID") %>' Style="display:none" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label ID="lblOVC_DBID_tw" Text='<%# Bind("OVC_DBID_tw") %>' CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label ID="lblOVC_BID_HOUR" Text='<%# Bind("OVC_BID_HOUR") %>' CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label  CssClass="control-label" runat="server">時</asp:Label>
                                            <asp:Label ID="lblOVC_BID_MIN" Text='<%# Bind("OVC_BID_MIN") %>' CssClass="control-label" runat="server"></asp:Label>
                                            <asp:Label  CssClass="control-label" runat="server">分</asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="審查結果" DataField="OVC_RESULT" />
                                </Columns>
                            </asp:GridView>

                             <div class="text-center">
                                <asp:Button ID="btnShowDivTBM1313_ITEM" Text="回上一頁" OnClick="btnShowDivTBM1313_ITEM_Click" CssClass="btn-default btnw6" runat="server" />
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

