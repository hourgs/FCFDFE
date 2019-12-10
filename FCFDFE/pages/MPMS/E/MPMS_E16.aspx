<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_E16.aspx.cs" Inherits="FCFDFE.pages.MPMS.E.MPMS_E16" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>

    <div class="row">
        <div style="width: 1000px; margin:auto;">
            <section class="panel">
                <header  class="title">
                    <!--標題-->契約管理
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel><!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style=" border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                            <!--網頁內容-->
                            <div class="text-center">
                                <asp:Button ID="btnApplyTaxFree" CssClass="btn-success" OnClick="btnApplyTaxFree_Click" runat="server" Text="申請免稅" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnTBMAUDIT" CssClass="btn-success" OnClick="btnTBMAUDIT_Click" runat="server" Text="履約督導" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnDeliveryAcceptance" CssClass="btn-success" OnClick="btnDeliveryAcceptance_Click" runat="server" Text="交貨暨驗收情況" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnTBMAUDIT_INSPECT" CssClass="btn-success" OnClick="btnTBMAUDIT_INSPECT_Click" runat="server" Text="督導會驗" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnTBMCONTRACT_MODIFY" CssClass="btn-success" OnClick="btnTBMCONTRACT_MODIFY_Click" runat="server" Text="契約修訂" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnMarginRefund" CssClass="btn-success" OnClick="btnMarginRefund_Click" runat="server" Text="保證(固)金收退" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnPrintCheckList" CssClass="btn-success" OnClick="btnPrintCheckList_Click" runat="server" Text="列印檢查項目表" />
                                <br><br>
                                <asp:LinkButton OnClick="Unnamed_Click" runat="server">列印物資申請書.doc</asp:LinkButton>
                                <asp:Button ID="btnMaterialAapply" Visible="false" CssClass="btn-success" OnClick="btnMaterialAapply_Click" runat="server" Text="列印物資申請書" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton OnClick="Unnamed_Click2" runat="server">列印採購計畫清單.doc</asp:LinkButton>
                                <asp:Button ID="btnPrintBuyList" Visible="false" CssClass="btn-success" runat="server" OnClick="btnPrintBuyList_Click" Text="列印採購計畫清單" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton OnClick="btnPrintTimeControl_Click" runat="server">列印時程管制表.doc</asp:LinkButton>
                                <asp:Button ID="btnPrintTimeControl" Visible="false" CssClass="btn-success" OnClick="btnPrintTimeControl_Click" runat="server" Text="列印時程管制表" />
                                <br>
                                <asp:LinkButton OnClick="btnMaterialAapply_Click" runat="server">列印物資申請書.pdf</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton OnClick="btnPrintBuyList_Click" runat="server">列印採購計畫清單.pdf</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton OnClick="Unnamed_Click4" runat="server">列印時程管制表.pdf</asp:LinkButton>
                                <br>
                                <asp:LinkButton OnClick="Unnamed_Click1" runat="server">列印物資申請書.odt</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton OnClick="Unnamed_Click3" runat="server">列印採購計畫清單.odt</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton OnClick="Unnamed_Click5" runat="server">列印時程管制表.odt</asp:LinkButton>
                                <br><br>
                            </div>
                            <table class="table table-bordered text-center">
                                <tr>
                                    <td rowspan="32" class="td-vertical"><asp:Label CssClass="text-vertical-s" style="height: 190px;" runat="server" Text="基本資料"></asp:Label></td>
                                    <td style="width:115px"><asp:Label CssClass="control-label" runat="server" Text="購案編號"></asp:Label></td>
                                    <td colspan="2" class="text-left"><asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="組別"></asp:Label></td>
                                    <td class="text-left"><asp:Label ID="lblONB_GROUP" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="購案名稱"></asp:Label></td>
                                    <td colspan="4" class="text-left"><asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="契約金額"></asp:Label></td>
                                    <td colspan="4" class="text-left"><asp:Label ID="lblOVC_CURRENT" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtONB_MONEY" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="簽約日期"></asp:Label></td>
                                    <td  colspan="2" class="text-left"><asp:Label ID="lblOVC_CONTRACT_START" CssClass="control-label" runat="server"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="收辦日期"></asp:Label></td>
                                    <td class="text-left"><asp:Label ID="lblOVC_DRECEIVE" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="5"><asp:Label CssClass="control-label" runat="server" Text="廠商資訊"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="廠商名稱"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_VEN_TITLE" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="廠商地址"></asp:Label></td>
                                    <td  colspan="2" class="text-left"><asp:TextBox ID="txtOVC_VEN_ADDRESS" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td rowspan="2"><asp:Label CssClass="control-label" runat="server" Text="聯絡方式"></asp:Label></td>
                                    <td><asp:Label ID="lblOVC_PUR_IUSER_PHONE" CssClass="control-label" runat="server" Text="自動電話"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="TextBox3" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="行動電話"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_VEN_TEL" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="傳真號碼"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtlOVC_VEN_FAX" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="電子郵件"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_VEN_EMAIL" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="5"><asp:Label CssClass="control-label" runat="server" Text="委購單位資訊"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="委購單位名稱"></asp:Label></td>
                                    <td colspan="2" class="text-left"><asp:TextBox ID="txtOVC_PUR_NSECTION" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="委購單位承辦人"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="TextBox2" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td rowspan="3"><asp:Label CssClass="control-label" runat="server" Text="聯絡方式"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="軍用電話"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_PUR_IUSER_PHONE_EXT" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="自動電話"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_PUR_IUSER_PHONE" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="行動電話"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_VEN_CELLPHONE" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="傳真號碼"></asp:Label></td>
                                    <td class="text-left"><asp:TextBox ID="txtOVC_VEN_FAX" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="電子郵件"></asp:Label></td>
                                    <td class="text-left" colspan="3"><asp:TextBox ID="txtOVC_VEN_EMAIL_1" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="驗收方式"></asp:Label></td>
                                    <td colspan="4" class="text-left">
                                        <asp:CheckBoxList ID="chkAcceptance" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>國外出口公証檢驗</asp:ListItem>
                                            <asp:ListItem>指定國外專業檢驗單位檢驗</asp:ListItem>
                                            <asp:ListItem>國內進口公証檢驗</asp:ListItem>
                                            <asp:ListItem>目視檢查</asp:ListItem>
                                            <asp:ListItem>儀器化驗</asp:ListItem>
                                            <asp:ListItem>性能測試</asp:ListItem>
                                            <asp:ListItem>品質保証文件</asp:ListItem>
                                            <asp:ListItem>教育訓練</asp:ListItem>
                                            <asp:ListItem>無</asp:ListItem>
                                            <asp:ListItem>備註</asp:ListItem>
                                        </asp:CheckBoxList>
                                        <asp:TextBox ID="txtOVC_INSPECT_REMARK" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                        <p><asp:Label ID ="labWay" CssClass="control-label text-blue position-left" runat="server"></asp:Label>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID ="labWay2" CssClass="control-label text-blue position-left" runat="server"></asp:Label></p>
                                    </td>
                                </tr>
                                <tr>
                                     <td><asp:Label CssClass="control-label" runat="server" Text="下授方式"></asp:Label></td>
                                     <td colspan="4" class="text-left">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                            <asp:DropDownList ID="drpOVC_GRANT_TO" OnSelectedIndexChanged="drpOVC_GRANT_TO_SelectedIndexChanged" CssClass="tb tb-l" AutoPostBack="True" runat="server"></asp:DropDownList>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="labUndergraduate" Visible="false" CssClass="control-label" runat="server" Text="下授人員："></asp:Label>
                                            <asp:DropDownList ID="drpUndergraduate" Visible="false" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                            <asp:UpdatePanel ID="upBegin" Visible="false" UpdateMode="Conditional" style="display:inline" runat="server">
                                               <ContentTemplate>
                                                   <p></p>
                                                   <asp:Label CssClass="control-label position-left" runat="server" Text="授權開始時間："></asp:Label>
                                                   <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                       <asp:TextBox ID="txtAuthorization_starttime" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                       <div class="add-on"><i class="icon-calendar"></i></div>
                                                   </div>
                                                   <asp:Button ID="btnAuthorization_starttime" CssClass="btn-default btnw4 position-left" OnClick="btnAuthorization_starttime_Click" runat="server" Text="清除日期" />
                                               </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:Label ID="labBlank" Visible="false" CssClass="control-label position-left" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Label>
                                            <asp:UpdatePanel ID="upBeginTime" Visible="false" UpdateMode="Conditional" style="display:inline" runat="server">
                                               <ContentTemplate>
                                                   <asp:Label CssClass="control-label position-left" runat="server" Text="授權結束時間："></asp:Label>
                                                   <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                       <asp:TextBox ID="txtAuthorization_endtime" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                       <div class="add-on"><i class="icon-calendar"></i></div>
                                                   </div>
                                                   <asp:Button ID="btnAuthorization_endtime" CssClass="btn-default btnw4 position-left" OnClick="btnAuthorization_endtime_Click" runat="server" Text="清除日期" />
                                                   <p><asp:Label ID ="labWay3" CssClass="control-label text-blue position-left" runat="server"></asp:Label></p>
                                               </ContentTemplate>
                                            </asp:UpdatePanel>
                                         </ContentTemplate>
                                     </asp:UpdatePanel>
                                     </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="結案日期"></asp:Label></td>
                                        <td colspan="2">
                                            <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                    <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd">
                                                        <asp:TextBox ID="txtOVC_DCLOSE" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                                        <div class="add-on"><i class="icon-calendar"></i></div>
                                                    </div>
                                                    <asp:Label CssClass="control-label text-blue position-left" runat="server"></asp:Label>
                                                    <asp:Button ID="btnTransferReset" CssClass="btn-default btnw4 position-left" OnClick="btnTransferReset_Click" runat="server" Text="清除日期" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="歸檔"></asp:Label></td>
                                    <td class="text-left"><asp:DropDownList ID="drpOVC_CLOSE" CssClass="tb tb-xs" runat="server">
                                        <asp:ListItem>是</asp:ListItem>
                                        <asp:ListItem>否</asp:ListItem></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="履驗承辦人"></asp:Label></td>
                                    <td colspan="2" class="text-left"><asp:DropDownList ID="drpOVC_DO_NAME" CssClass="tb tb-s" runat="server"></asp:DropDownList></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="履約督導"></asp:Label></td>
                                    <td class="text-left"><asp:DropDownList ID="drpOVC_INSPECT" CssClass="tb tb-xs" runat="server">
                                        <asp:ListItem>是</asp:ListItem>
                                        <asp:ListItem>否</asp:ListItem></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="延約條款"></asp:Label></td>
                                    <td colspan="4" class="text-left"><asp:RadioButtonList ID="rdoOVC_CONTRACT_DELAY" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                    </asp:RadioButtonList>
                                        <asp:TextBox ID="txtOVC_DELAY_REASON" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="後續擴充條款條款"></asp:Label></td>
                                    <td colspan="4" class="text-left"><asp:RadioButtonList ID="rdoOVC_RESERVE" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                        <asp:ListItem>是</asp:ListItem>
                                                        <asp:ListItem>否</asp:ListItem>
                                                    </asp:RadioButtonList>
                                        <asp:TextBox ID="txtOVC_RESERVE_REASON" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="4"><asp:Label CssClass="control-label" runat="server" Text="履約金"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="代管現金："></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblTBMMANAGE_CASH" CssClass="control-label" runat="server">0.00</asp:Label>
                                        <asp:Label CssClass="control-label" runat="server" Text="元"></asp:Label></td>
                                    <td rowspan="3"><asp:Label ID="Label36" CssClass="control-label" runat="server" Text="履保金期限"></asp:Label></td>
                                    <td><asp:Label ID="lblTBMMANAGE_CASH_DATE" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="代管文件："></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblTBMMANAGE_PROM" CssClass="control-label" runat="server">0.00</asp:Label>
                                        <asp:Label CssClass="control-label" runat="server" Text="元"></asp:Label>
                                    </td>
                                    <td><asp:Label ID="lblTBMMANAGE_PROM_DATE" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="代管有價證券："></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblTBMMANAGE_STOCK" CssClass="control-label" runat="server">0.00</asp:Label>
                                        <asp:Label CssClass="control-label" runat="server" Text="元"></asp:Label>
                                    </td>
                                    <td><asp:Label ID="lblTBMMANAGE_STOCK_DATE" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="text-left">
                                        <asp:Label ID="labWay5" CssClass="control-label text-blue" runat="server" Text=""></asp:Label>
                                        <asp:Label ID="lblONB_MCONTRACT" CssClass="control-label text-blue" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="4"><asp:Label CssClass="control-label" runat="server" Text="保固金"></asp:Label></td>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="代管現金："></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblTBMMANAGE_CASH_2" CssClass="control-label" runat="server">0.00</asp:Label>
                                        <asp:Label CssClass="control-label" runat="server" Text="元"></asp:Label></td>
                                    <td rowspan="3"><asp:Label ID="Label2" CssClass="control-label" runat="server" Text="保固金期限"></asp:Label></td>
                                    <td><asp:Label ID="lblTBMMANAGE_CASH_DATE_2" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="代管文件："></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblTBMMANAGE_PROM_2" CssClass="control-label" runat="server">0.00</asp:Label>
                                        <asp:Label CssClass="control-label" runat="server" Text="元"></asp:Label>
                                    </td>
                                    <td><asp:Label ID="lblTBMMANAGE_PROM_DATE_2" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="代管有價證券："></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblTBMMANAGE_STOCK_2" CssClass="control-label" runat="server">0.00</asp:Label>
                                        <asp:Label CssClass="control-label" runat="server" Text="元"></asp:Label>
                                    </td>
                                    <td><asp:Label ID="lblTBMMANAGE_STOCK_DATE_2" CssClass="control-label" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="text-left">
                                        <asp:Label ID="labWay5_2" CssClass="control-label text-blue" runat="server" Text=""></asp:Label>
                                        <asp:Label ID="lblONB_MCONTRACT_2" CssClass="control-label text-blue" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                    
                                <tr>
                                    <td><asp:Label CssClass="control-label" runat="server" Text="相關稅賦"></asp:Label></td>
                                    <td colspan="2">
                                        <asp:CheckBoxList ID="chkOVC_PUR_FEE_OK" CssClass="radioButton text-green" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem>免關稅</asp:ListItem>
                                            <asp:ListItem>免進口營業額</asp:ListItem>
                                            <asp:ListItem>免進口貨物稅</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                    <td>
                                        <asp:Label  CssClass="control-label" runat="server" Text="國內銷售："></asp:Label><br>
                                        <asp:CheckBox ID="chkOVC_PUR_TAX_OK" CssClass="text-green" runat="server" Text="免營業稅"/>
                                    </td>
                                    <td>
                                        <asp:Label  CssClass="control-label" runat="server" Text="國內製造："></asp:Label>
                                        <asp:CheckBox ID="chkOVC_PUR_GOOD_OK" CssClass="text-green" runat="server" Text="免貨物稅"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td-vertical"><asp:Label CssClass="control-label text-vertical-m" style="height: 210px;" runat="server" Text="重要事項記載"></asp:Label></td>
                                    <td colspan="4"><asp:TextBox ID="txtOVC_RECEIVE_COMM" CssClass="tb tb-full" runat="server" TextMode="MultiLine" Height="210px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <div style="display:inline-block" onmouseover="document.getElementById('div1').style.display = 'block';document.getElementById('div2').style.display = 'none';document.getElementById('div3').style.display = 'none';">
                                            <asp:Button ID="btnOVC_SHIP_TIMES" CssClass="btn-success" runat="server" Text="契約交貨時間" /></div>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <div style="display:inline-block" onmouseover="document.getElementById('div2').style.display = 'block';document.getElementById('div1').style.display = 'none';document.getElementById('div3').style.display = 'none';">
                                            <asp:Button ID="btnOVC_RECEIVE_PLACE" CssClass="btn-success" runat="server" Text="契約交貨地點" /></div>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <div style="display:inline-block" onmouseover="document.getElementById('div3').style.display = 'block';document.getElementById('div1').style.display = 'none';document.getElementById('div2').style.display = 'none';">
                                            <asp:Button ID="btnOVC_PAYMENT" CssClass="btn-success" runat="server" Text="付款方式" /></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <div id="div1" style="display: none;">
                                            <asp:Label ID="lblONB_MCONTRACTPer1_1" CssClass="control-label" runat="server"></asp:Label>
                                            <p style="TEXT-ALIGN:left;"><asp:Label ID="lblONB_MCONTRACTPer1_2" CssClass="control-label text-blue" runat="server"></asp:Label></p>
                                        </div>
                                        <div id="div2" style="display: none;">
                                            <asp:Label ID="lblONB_MCONTRACTPer2_1" CssClass="control-label" runat="server"></asp:Label>
                                            <p style="TEXT-ALIGN:left;"><asp:Label ID="lblONB_MCONTRACTPer2_2" CssClass="control-label text-blue" runat="server"></asp:Label></p>
                                        </div>
                                        <div id="div3" style="display: none;">
                                            <asp:Label ID="lblONB_MCONTRACTPer3_1" CssClass="control-label" runat="server"></asp:Label>
                                            <p style="TEXT-ALIGN:left;"><asp:Label ID="lblONB_MCONTRACTPer3_2" CssClass="control-label text-blue" runat="server"></asp:Label></p>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div class="text-center">
                                        <asp:Button ID="btnReturnM" CssClass="btn-default btnw4" OnClick="btnReturnM_Click" runat="server" Text="回主流程" />
                                        <asp:Button ID="btnSave" CssClass="btn-warning btnw2" OnClick="btnSave_Click" runat="server" Text="存檔" /><br><br>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSave" />
                                </Triggers>
                            </asp:UpdatePanel>
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
