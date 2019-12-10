<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E28.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E28" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <style>
        tr td:nth-child(2n) {
            text-align: left;
        }
    </style>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    驗收情形報告
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">

                            <table id="tb" class="table table-bordered text-center" runat="server">
                                <tr>
                                    <th colspan="4" style="background: red;">
                                        <asp:Label ForeColor="#ffff37" Font-Size="X-Large" CssClass="control-label" runat="server">請注意！未輸入資料或存檔無法列印各項報表！！</asp:Label></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server">GH0613L008PE</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">批次 </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtONB_SHIP_TIMES" CssClass="tb text-red tb-s" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">複驗次數</asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="drpONB_INSPECT_TIMES" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>0</asp:ListItem>
                                            <asp:ListItem>1</asp:ListItem>
                                            <asp:ListItem>2</asp:ListItem>
                                            <asp:ListItem>3</asp:ListItem>
                                            <asp:ListItem>4</asp:ListItem>
                                            <asp:ListItem>5</asp:ListItem>
                                            <asp:ListItem>6</asp:ListItem>
                                            <asp:ListItem>7</asp:ListItem>
                                            <asp:ListItem>8</asp:ListItem>
                                            <asp:ListItem>9</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                    </asp:DropDownList></td>
                                    <td>
                                        <asp:Label CssClass="control-label" runat="server">再驗次數 </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpONB_RE_INSPECT_TIMES" CssClass="tb tb-s" runat="server">
                                            <asp:ListItem>0</asp:ListItem>
                                            <asp:ListItem>1</asp:ListItem>
                                            <asp:ListItem>2</asp:ListItem>
                                            <asp:ListItem>3</asp:ListItem>
                                            <asp:ListItem>4</asp:ListItem>
                                            <asp:ListItem>5</asp:ListItem>
                                            <asp:ListItem>6</asp:ListItem>
                                            <asp:ListItem>7</asp:ListItem>
                                            <asp:ListItem>8</asp:ListItem>
                                            <asp:ListItem>9</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                    </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server">軍品名稱及數量</asp:Label></td>
                                    <td colspan="3"><asp:TextBox ID="txtOVC_REPORT_DESC" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td rowspan="5"><asp:Label CssClass="control-label" runat="server">驗收情形摘要</asp:Label></td>
                                    <td colspan="3"><asp:Label CssClass="control-label" runat="server">一、交驗軍品數量</asp:Label>
                                        <asp:RadioButtonList ID="rdoOVC_RESULT_1" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="Y">會同清點大數相符</asp:ListItem>
                                            <asp:ListItem Value="N">與合約規定不符</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="display:none;"></td>
                                    <td colspan="3"><asp:Label CssClass="control-label" runat="server">二、抽驗<asp:TextBox ID="txtOVC_RESULT_2_PERCENT" CssClass="tb tb-s" runat="server"></asp:TextBox>%目視情形</asp:Label>
                                        <asp:RadioButtonList ID="rdoOVC_RESULT_2" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="Y">與合約規定相符</asp:ListItem>
                                            <asp:ListItem Value="N">與合約規定不符</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td style="display:none;"></td>
                                    <td colspan="3"><asp:Label CssClass="control-label" runat="server">三、包裝情形</asp:Label>
                                        <asp:RadioButtonList ID="rdoOVC_RESULT_3" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="Y">與合約規定相符</asp:ListItem>
                                            <asp:ListItem Value="N">與合約規定不符</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td style="display:none;"></td>
                                    <td colspan="3"><asp:Label CssClass="control-label" runat="server">四、交貨時間</asp:Label>
                                        <asp:RadioButtonList ID="rdoOVC_RESULT_4" CssClass="radioButton" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                            <asp:ListItem Value="Y">未逾期</asp:ListItem>
                                            <asp:ListItem Value="N">逾期</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:TextBox ID="txtOVC_RESULT_4_DELAY" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                        <asp:Label CssClass="control-label" runat="server">天罰款：</asp:Label>
                                        <asp:TextBox ID="txtOVC_RESULT_4_PUNISH" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td style="display:none;"></td>
                                    <td colspan="3"><asp:Label CssClass="control-label" runat="server">五、其他</asp:Label>
                                        <asp:TextBox ID="txtOVC_RESULT_5" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="display:none;"></td>
                                    <td colspan="4"><asp:Label CssClass="control-label text-red" Font-Size="X-Large" Font-Bold="true" runat="server">擬辦</asp:Label><br />
                                        <asp:Label CssClass="control-label" Font-Size="Large" Font-Bold="true" runat="server">內購案</asp:Label><br />
                                        
                                        <asp:CheckBoxList ID="CheckBoxList1" CssClass="radioButton text-blue" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:Label ID="label_1" Text="1.俟承商出具軍品保證書後准予完成驗收" CssClass="control-label text-blue" runat="server"></asp:Label><br />
                                        <asp:CheckBoxList ID="CheckBoxList2" CssClass="radioButton text-blue" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:Label ID="label_2" Text="2.俟檢驗合格並由承商出具軍品保證書後准予驗收" CssClass="control-label text-blue" runat="server"></asp:Label><br />
                                        <asp:CheckBoxList ID="CheckBoxList3" CssClass="radioButton text-blue" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:Label ID="label_3" Text="3.俟檢驗合格並由承商辦妥交貨手續及出具軍品保證書後准予完成驗收" CssClass="control-label text-blue" runat="server"></asp:Label><br />
                                        <asp:CheckBoxList ID="CheckBoxList4" CssClass="radioButton text-blue" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:Label ID="label_4" Text="4.俟檢驗合格並由承商辦妥交貨手續後准予完成驗收" CssClass="control-label text-blue" runat="server"></asp:Label><br />
                                        <asp:CheckBoxList ID="CheckBoxList5" CssClass="radioButton text-blue" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:Label ID="label_5" Text="5.俟紗支(材料)抽驗合格後通知承商按合約規定辦理並副知各有關單位" CssClass="control-label text-blue" runat="server"></asp:Label><br />
                                        <asp:CheckBoxList ID="CheckBoxList6" CssClass="radioButton text-blue" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:Label ID="label_6" Text="6.俟安裝試驗(用)合格由接收單位函告本局由承商出具軍品保證書後准予完成驗收" CssClass="control-label text-blue" runat="server"></asp:Label><br />
                                        <asp:CheckBoxList ID="CheckBoxList7" CssClass="radioButton text-blue" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:Label ID="label_7" Text="7.俟抽樣檢驗與試裝(用)合格並由承商出具軍品保證書後准予完成驗收" CssClass="control-label text-blue" runat="server"></asp:Label><br />
                                        <asp:CheckBoxList ID="CheckBoxList8" CssClass="radioButton text-blue" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:Label ID="label_8" Text="8.經抽驗目視檢查與合約規定不符經協議本次不予驗收並限承商於" CssClass="control-label text-blue" runat="server"></asp:Label>
                                        <asp:TextBox ID="TextBox_8" runat="server"></asp:TextBox>
                                        <asp:Label ID="label2_8" Text="天內退(換)或重新報驗" CssClass="control-label text-blue" runat="server"></asp:Label><br />
                                        <asp:CheckBoxList ID="CheckBoxList9" CssClass="radioButton text-blue" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:Label ID="label_9" Text="9.未按規定包裝經協議本次不予驗收並限承商於" CssClass="control-label text-blue" runat="server"></asp:Label>
                                        <asp:TextBox ID="TextBox_9" runat="server"></asp:TextBox>
                                        <asp:Label ID="label2_9" Text="天內整理完竣重新報驗，如已逾交貨時間仍應按規定計罰" CssClass="control-label text-blue" runat="server"></asp:Label><br />
                                        <asp:CheckBoxList ID="CheckBoxList10" CssClass="radioButton text-blue" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:Label ID="label_10" Text="10.經目視抽驗發現尚有輕微缺點經協議限承商於" CssClass="control-label text-blue" runat="server"></asp:Label>
                                        <asp:TextBox ID="TextBox_10" runat="server"></asp:TextBox>
                                        <asp:Label ID="label2_10" Text="天內整理完竣重新報驗如逾交貨時間仍應按規定計罰" CssClass="control-label text-blue" runat="server"></asp:Label><br />
                                        <asp:CheckBoxList ID="CheckBoxList11" CssClass="radioButton text-blue" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:Label ID="label_11" Text="11.現場堆積紊亂一時無法清點經協議限承商於" CssClass="control-label text-blue" runat="server"></asp:Label>
                                        <asp:TextBox ID="TextBox_11" runat="server"></asp:TextBox>
                                        <asp:Label ID="label2_11" Text="天內整理完竣重新報驗如逾交貨時間仍應按規定計罰" CssClass="control-label text-blue" runat="server"></asp:Label><br />
                                        <asp:CheckBoxList ID="CheckBoxList12" CssClass="radioButton text-blue" RepeatLayout="Flow" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:Label Text="12.其他" CssClass="control-label text-blue" runat="server"></asp:Label><br />

                                        <asp:CheckBoxList ID="chkONB_ITEM" CssClass="radioButton text-blue" RepeatLayout="UnorderedList" Visible="false" runat="server">
                                        <asp:ListItem Value="值">1. 俟承商出具軍品保證書後准予完成驗收</asp:ListItem>
                                        <asp:ListItem Value="值">2. 俟檢驗合格並由承商出具軍品保證書後准予驗收 </asp:ListItem>
                                        <asp:ListItem Value="值">3. 俟檢驗合格並由承商辦妥交貨手續及出具軍品保證書後准予完成驗收</asp:ListItem>
                                        <asp:ListItem Value="值">4. 俟檢驗合格並由承商辦妥交貨手續後准予完成驗收</asp:ListItem>
                                        <asp:ListItem Value="值">5. 俟紗支(材料)抽驗合格後通知承商按合約規定辦理並副知各有關單位</asp:ListItem>
                                        <asp:ListItem Value="值">6. 俟安裝試驗(用)合格由接收單位函告本局由承商出具軍品保證書後准予完成驗收</asp:ListItem>
                                        <asp:ListItem Value="值">7. 俟抽樣檢驗與試裝(用)合格並由承商出具軍品保證書後准予完成驗收</asp:ListItem>
                                        <asp:ListItem Value="值">8. 經抽驗目視檢查與合約規定不符經協議本次不予驗收並限承商於(要放TB)天內退(換)或重新報驗</asp:ListItem>
                                        <asp:ListItem Value="值">9. 未按規定包裝經協議本次不予驗收並限承商於(要放TB)天內整理完竣重新報驗，如已逾交貨時間仍應按規定計罰</asp:ListItem>
                                        <asp:ListItem Value="值">10.經目視抽驗發現尚有輕微缺點經協議限承商於(要放TB)天內整理完竣重新報驗如逾交貨時間仍應按規定計罰</asp:ListItem>
                                        <asp:ListItem Value="值">11.現場堆積紊亂一時無法清點經協議限承商於(要放TB)天內整理完竣重新報驗如逾交貨時間仍應按規定計罰</asp:ListItem>
                                        <asp:ListItem Value="值">12.其他</asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:TextBox ID="txtOther" TextMode="MultiLine" Rows="5" CssClass="textarea tb-full" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>

                            <div class="text-center">
                                <asp:Button ID="btnReturn" CssClass="btn-default btnw4" OnClick="btnReturn_Click" runat="server" Text="回上一頁" />
                                <asp:Button ID="btnReturnM" CssClass="btn-default btnw4" OnClick="btnReturnM_Click" runat="server" Text="回主流程" />
                                <asp:Button ID="btnSave" CssClass="btn-default btnw2" OnClick="btnSave_Click" runat="server" Text="存檔" />
                            </div>
                            <br />
                            <div class="text-center">
                                <asp:LinkButton ID="LinkButton1" OnClick="LinkButton1_Click" CssClass="text-red" runat="server">列印驗收情形報告.doc</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton2" OnClick="LinkButton2_Click" CssClass="text-red" runat="server">列印驗收情形報告.pdf</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButton3" OnClick="LinkButton3_Click" CssClass="text-red" runat="server">列印驗收情形報告.odt</asp:LinkButton>
                                <asp:HyperLink ID="InkPreview" Visible="false" CssClass="text-red" runat="server">列印檢驗申請單</asp:HyperLink>
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
