<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D1D.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D1D" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <script>
        function OpenQC(index) {
            var win_width = 1000;
            var win_height = 500;
            var PosX = (screen.width - win_width) / 2;
            var PosY = (screen.Height - win_height) / 2;
            features = "width=" + win_width + ",height=" + win_height + ",top=" + PosY + ",left=" + PosX;
            var OVC_PURCH = '<%=sOVC_PURCH%>';
            var OVC_PURCH_5 = '<%=sOVC_PURCH_5%>';
            if (index == 1) {
                var control_name = document.getElementById('<%=txtEscOVC_OWN_NAME.ClientID%>').id;
                var control_cst = document.getElementById('<%=txtEscOVC_OWN_NO.ClientID%>').id;
                var control_address = document.getElementById('<%=txtEscOVC_OWN_ADDRESS.ClientID%>').id;
                var theURL = '<%=ResolveClientUrl("QueryCompany.aspx?OVC_PURCH=")%>' + OVC_PURCH + '&OVC_PURCH_5=' + OVC_PURCH_5 + '&NAME=' + control_name + '&CST=' + control_cst + '&ADDRESS=' + control_address;
                var newwin = window.open(theURL, 'unitQuery', features);
            }
            if (index == 2) {
                var control_name = document.getElementById('<%=txtDocOVC_OWN_NAME.ClientID%>').id;
                var control_address = document.getElementById('<%=txtDocOVC_OWN_ADDRESS.ClientID%>').id;
                var theURL = '<%=ResolveClientUrl("QueryCompany.aspx?OVC_PURCH=")%>' + OVC_PURCH + '&OVC_PURCH_5=' + OVC_PURCH_5 + '&NAME=' + control_name + '&ADDRESS=' + control_address;
                var newwin = window.open(theURL, 'unitQuery', features);
            }
            if (index == 3) {
                var control_name = document.getElementById('<%=txtSecOVC_OWN_NAME.ClientID%>').id;
                var control_cst = document.getElementById('<%=txtSecOVC_OWN_NO.ClientID%>').id;
                var control_address = document.getElementById('<%=txtSecOVC_OWN_ADDRESS.ClientID%>').id;
                var theURL = '<%=ResolveClientUrl("QueryCompany.aspx?OVC_PURCH=")%>' + OVC_PURCH + '&OVC_PURCH_5=' + OVC_PURCH_5 + '&NAME=' + control_name + '&CST=' + control_cst + '&ADDRESS=' + control_address;
                var newwin = window.open(theURL, 'unitQuery', features);
            }
        }
    </script>

    <div class="row">
        <div style="width: 1000px; margin: auto;">
            <section class="panel">
                <header class="title">
                    <!--標題-->
                    <asp:label ID="lblTitle" runat="server">保證金(函)收繳</asp:label>
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" id="divForm" style="border: solid 2px;" visible="false" runat="server">
                    <div class="form" style="border: 5px;">

                        <asp:Panel ID="PnContract" runat="server">
                            <table class="table table-bordered text-center">
                                <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="購案編號"></asp:Label></td>
                                        <td class="text-left"><asp:Label ID="lblOVC_PURCH_1" CssClass="control-label" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="契約編號"></asp:Label></td>
                                        <td class="text-left">
                                            <asp:TextBox ID="txtOVC_PURCH_6" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                            <asp:Label runat="server" Text="組別："></asp:Label>
                                            <asp:TextBox ID="txtONB_GROUP" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="名稱"></asp:Label></td>
                                        <td class="text-left"><asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="預算金額"></asp:Label></td>
                                        <td class="text-left">
                                            <asp:DropDownList ID="drpOVC_BUD_CURRENT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                            <asp:TextBox ID="txtONB_BUD_MONEY" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                            <asp:Label CssClass="control-label" runat="server" Text="元整"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="決標日期"></asp:Label></td>
                                        <td class="text-left">
                                            <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                                <asp:TextBox ID="txtOVC_DBID" CssClass="tb tb-s position-left" runat="server" readonly="true"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                            </div>
                                        </td>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="簽約日期"></asp:Label></td>
                                        <td class="text-left">
                                            <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                                <asp:TextBox ID="txtOVC_DCONTRACT" CssClass="tb tb-s position-left" runat="server" readonly="true"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="開標日期"></asp:Label></td>
                                        <td class="text-left">
                                            <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                                <asp:TextBox ID="txtOVC_DOPEN" CssClass="tb tb-s position-left" runat="server" readonly="true"></asp:TextBox>
                                                <div class="add-on"><i class="icon-calendar"></i></div>
                                            </div>
                                        </td>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="折讓金額"></asp:Label></td>
                                        <td class="text-left"><asp:TextBox ID="txtONB_MONEY_DISCOUNT" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="契約金額"></asp:Label></td>
                                        <td class="text-left">
                                            <asp:DropDownList ID="drpOVC_CURRENT" CssClass="tb tb-s" runat="server"></asp:DropDownList>
                                            <asp:TextBox ID="txtONB_MONEY" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                            <asp:Label CssClass="control-label" runat="server" Text="元整"></asp:Label></td>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="交貨時間"></asp:Label></td>
                                        <td class="text-left"><asp:TextBox ID="txtOVC_SHIP_TIMES" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="交貨地點"></asp:Label></td>
                                        <td class="text-left"><asp:TextBox ID="txtOVC_RECEIVE_PLACE" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="交貨批次"></asp:Label></td>
                                        <td class="text-left"><asp:TextBox ID="txtONB_DELIVERY_TIMES" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="付款方式"></asp:Label></td>
                                        <td class="text-left"><asp:TextBox ID="txtOVC_PAYMENT" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    </tr>
                            </table>
                            <div class="cmxform form-horizontal tasi-form">
                                <asp:GridView ID="GV_TBM1302" DataKeyNames="OVC_PURCH_6" CssClass="table data-table table-striped border-top text-center" OnRowCommand="GV_TBM1302_RowCommand" OnPreRender="GV_TBM1302_PreRender" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:Button ID="btnCompany" Text="異動" CssClass="btn-default" CommandName="btnComMod" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="合約編號">
                                            <ItemTemplate>
                                                <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PURCH") %>' runat="server"></asp:Label>
                                                <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PUR_AGENCY") %>' runat="server"></asp:Label>
                                                <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PURCH_5") %>' runat="server"></asp:Label>
                                                <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PURCH_6") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="組別" DataField="ONB_GROUP" />
                                        <asp:BoundField HeaderText="得標商名稱" DataField="OVC_VEN_TITLE" />
                                        <asp:TemplateField HeaderText="作業">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnContractOffice" CommandName="lbtnOffice" runat="server">列印發包處通知單.doc</asp:LinkButton>
                                                <asp:LinkButton ID="lbtnContractOffice_odt" CommandName="lbtnOffice_odt" runat="server">.odt</asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="lbtnContractAttachment" CommandName="lbtnAttachment" runat="server">列印契約及附件分配表.doc</asp:LinkButton>
                                                <asp:LinkButton ID="lbtnContractAttachment_odt" CommandName="lbtnAttachment_odt" runat="server">.odt</asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:LinkButton ID="lbtnCheckItem" Visible="false" OnClick="lbtnCheckItem_Click" runat="server">列印檢查項目表</asp:LinkButton>
                                                <asp:Button ID="btnCheck" CommandName="btnChk" CssClass="btn-default" runat="server" Text="契約製作檢查項目編輯" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <asp:Panel ID="PnCompany" runat="server">
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
                                <table class="table table-bordered text-center">
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="廠商名稱"></asp:Label></td>
                                        <td class="text-left"><asp:TextBox ID="txtOVC_VEN_TITLE" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="廠商統一編號"></asp:Label></td>
                                        <td class="text-left">
                                            <asp:TextBox ID="txtOVC_VEN_CST" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                            <%--<asp:HyperLink ID="InkTBM1313" runat="server">查詢投標商資料</asp:HyperLink>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="廠商傳真"></asp:Label></td>
                                        <td class="text-left"><asp:TextBox ID="txtOVC_VEN_FAX" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="廠商電話"></asp:Label></td>
                                        <td class="text-left"><asp:TextBox ID="txtOVC_VEN_TEL" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="廠商EMAIL"></asp:Label></td>
                                        <td class="text-left"><asp:TextBox ID="txtOVC_VEN_EMAIL" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="負責人"></asp:Label></td>
                                        <td class="text-left"><asp:TextBox ID="txtOVC_VEN_BOSS" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="聯絡人"></asp:Label></td>
                                        <td class="text-left"><asp:TextBox ID="txtOVC_VEN_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="聯絡人手機"></asp:Label></td>
                                        <td class="text-left"><asp:TextBox ID="txtOVC_VEN_CELLPHONE" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="廠商地址"></asp:Label></td>
                                        <td class="text-left" colspan="3"><asp:TextBox ID="txtOVC_VEN_ADDRESS" CssClass="tb tb-full" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label CssClass="control-label" runat="server" Text="重要事項"></asp:Label></td>
                                        <td class="text-left" colspan="3"><asp:TextBox ID="txtOVC_CONTRACT_COMM" CssClass="tb tb-full" runat="server"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <%--<table class="table table-bordered">
                                <tr>
                                    <td colspan="3" style="text-align:right;Width:50%;border-right:none">
                                        <asp:Label CssClass="control-label text-red" runat="server">簽約日：</asp:Label>
                                    </td>
                                    <td colspan="3" style="text-align:left;Width:50%;border-left: none;">
                                        <div class="input-append datepicker position-left" style="border-left-style:none;border-left:none">
                                            <asp:TextBox ID="txtOVC_DCONTRACT" CssClass="tb tb-s position-left text-change" runat="server"></asp:TextBox>
                                            <span class='add-on'><i class="icon-calendar"></i></span>
                                        </div>
                                    </td>
                                </tr>
                            </table>--%>
                            <div style="text-align: center">
                                <asp:Button ID="btnEscrow" CssClass="btn-default" Text="保證金(函)收繳" OnClick="btnEscrow_Click" runat="server" />&nbsp;&nbsp;
                                <asp:Button ID="btnSaveCONTRACT" CssClass="btn-default" Text ="存檔" OnClick="btnSaveCONTRACT_Click" runat="server" />&nbsp;&nbsp;
                                <asp:Button ID="btnBack" OnClick="btnMain_Click" CssClass="btn-default" runat="server" Text="回主流程畫面" />
                            </div>
                            <br />
                            <div class="text-center">
                                <asp:LinkButton ID="lbtnContract" OnClick="lbtnContract_Click" runat="server">列印合約.doc</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lbtnContract_odt" OnClick="lbtnContract_odt_Click" runat="server">列印合約.odt</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lbtnLetterApplicationForm" OnClick="lbtnLetterApplicationForm_Click" runat="server">列印印信申請表.doc</asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lbtnLetterApplicationForm_odt" OnClick="lbtnLetterApplicationForm_odt_Click" runat="server">列印印信申請表.odt</asp:LinkButton>
                            </div>
                        </asp:Panel>
                        <br /><br />
                        
                        <div class="cmxform form-horizontal tasi-form">
                        <asp:Panel ID="PnEscrow" runat="server">
                            <!--網頁內容-->
                            <!--頁籤－開始-->
                            <header class="panel-heading">
                                <ul class="nav nav-tabs">
                                    <!--各頁籤-->
                                    <li class="active">
                                        <!--起始選取頁籤，只有一個-->
                                        <a data-toggle="tab" href="#TabEsc">代管現金收繳</a>
                                    </li>
                                    <li class="">
                                        <!--尚未選取頁籤-->
                                        <a data-toggle="tab" href="#TabDoc">代管保證文件</a>
                                    </li>
                                    <li class="">
                                        <!--尚未選取頁籤-->
                                        <a data-toggle="tab" href="#TabSec">代管有價證券</a>
                                    </li>
                                </ul>
                            </header>
                            <div class="panel-body tab-body">
                                <div class="tab-content">
                                    <!--各標籤之頁面-->

                                    <div id="TabEsc" class="tab-pane active">
                                        <!--起始選取頁面，只有一個-->
                                        <!--代管現金收繳-->
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:Panel ID="PnMessageC" runat="server"></asp:Panel>
                                                <asp:Panel ID="panCashD" runat="server">
                                                    <asp:GridView ID="GV_TBMMANAGE_CASH" DataKeyNames="OVC_PURCH_6" CssClass="table data-table table-striped border-top text-center" OnRowCommand="GV_TBMMANAGE_CASH_RowCommand" OnPreRender="GV_TBMMANAGE_CASH_PreRender" AutoGenerateColumns="false" runat="server">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="作業">
                                                                <ItemTemplate>
                                                                    <asp:Button CssClass="btn-default" Text="異動" CommandName="btnwork" runat="server" />
                                                                    <asp:Button CssClass="btn-warning" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" Text="刪除" Visible='<%#  Eval("OVC_DAPPROVE").ToString() != "" ? false:true %>' CommandName="btnDel" runat="server" /><br />
                                                                    <div visible='<%# Container.DisplayIndex + 1 == 1 ? false : true %>' runat="server">
                                                                        由項次:<asp:TextBox ID="txtCopy" CssClass="tb tb-s" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                                                        <asp:Button ID="btnCopy" CommandName="Copy" Text="複製" CssClass="btn-default" runat="server"></asp:Button>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="項次">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItem" CssClass="control-label" Text='<%#Container.DataItemIndex+1 %>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="類別" DataField="OVC_KIND" />
                                                            <asp:TemplateField HeaderText="合約編號">
                                                                <ItemTemplate>
                                                                    <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PURCH") %>' runat="server"></asp:Label>
                                                                    <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PUR_AGENCY") %>' runat="server"></asp:Label>
                                                                    <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PURCH_5") %>' runat="server"></asp:Label>
                                                                    <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PURCH_6") %>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="款項所有權人" DataField="OVC_OWN_NAME" />
                                                            <asp:BoundField HeaderText="合計金額" DataField="ONB_ALL_MONEY" />
                                                            <asp:BoundField HeaderText="收入單通知編號" DataField="OVC_RECEIVE_NO" />
                                                            <asp:BoundField HeaderText="收入日期" DataField="OVC_DRECEIVE" />
                                                        </Columns>
                                                    </asp:GridView>
                                                    <div style="text-align: center">
                                                        <asp:Button ID="btnNewC" OnClick="btnNewC_Click" CssClass="btn-default btnw2" runat="server" Text="新增" />
                                                        <br /><br />
                                                        <asp:Label ID="lblCashNo" CssClass="control-label" runat="server" Text="由編號："></asp:Label>
                                                        <asp:TextBox ID="txtCashNo" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                        <asp:Button ID="btnCashNo" Text="複製" OnClick="btnCashNo_Click" CssClass="btn-default btnw2" runat="server" />
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel ID="panCash" runat="server">
                                                    <table class="table table-bordered">
                                                        <tr>
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="購案合約編號："></asp:Label>
                                                                <asp:Label ID="lblEscOVC_PURCH_6" CssClass="control-label" runat="server"></asp:Label>
                                                                <asp:TextBox CssClass="tb tb-s" ID="txtEscOVC_PURCH_6" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:LinkButton ID="LinkButton1" OnClientClick="OpenQC(1)" runat="server">查詢本合約之合約商資料</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="款項所有權人："></asp:Label>
                                                                <asp:TextBox ID="txtEscOVC_OWN_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                                <asp:Label CssClass="control-label" runat="server" Text="款項所有權人統一編號："></asp:Label>
                                                                <asp:TextBox ID="txtEscOVC_OWN_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="款項所有權人地址："></asp:Label>
                                                                <asp:TextBox ID="txtEscOVC_OWN_ADDRESS" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="款項所有權人電話："></asp:Label>
                                                                <asp:TextBox ID="txtEscOVC_OWN_TEL" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td class="td-inner-table">
                                                                <asp:Label CssClass="control-label text-blue" runat="server" Text="代管項目"></asp:Label>
                                                                <asp:Label CssClass="control-label text-red" runat="server" Text="(最多三筆)"></asp:Label>
                                                                <table class="table table-inner table-bordered text-center">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label CssClass="control-label text-green" runat="server" Text="項次"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label CssClass="control-label text-green" runat="server" Text="代管事由說明"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label CssClass="control-label text-green" runat="server" Text="原幣別"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label CssClass="control-label text-green" runat="server" Text="原幣金額"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label CssClass="control-label text-green" runat="server" Text="新台幣入帳金額(必填)"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblEscONB_ITEM_1" CssClass="control-label text-green" runat="server" Text="1"></asp:Label></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEscOVC_REASON_1" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:DropDownList ID="drpEscOVC_CURRENT_1" CssClass="tb tb-s" runat="server"></asp:DropDownList></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEscONB_MONEY_1" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEscONB_MONEY_NT_1" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblEscONB_ITEM_2" CssClass="control-label text-green" runat="server" Text="2"></asp:Label></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEscOVC_REASON_2" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:DropDownList ID="drpEscOVC_CURRENT_2" CssClass="tb tb-s" runat="server"></asp:DropDownList></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEscONB_MONEY_2" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEscONB_MONEY_NT_2" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblEscONB_ITEM_3" CssClass="control-label text-green" runat="server" Text="3"></asp:Label></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEscOVC_REASON_3" CssClass="tb tb-l" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:DropDownList ID="drpEscOVC_CURRENT_3" CssClass="tb tb-s" runat="server"></asp:DropDownList></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEscONB_MONEY_3" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEscONB_MONEY_NT_3" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                    </tr>
                                                                </table>
                                                                <br />
                                                                <asp:Label CssClass="control-label text-red" runat="server" Text="合計金額："></asp:Label>
                                                                <asp:TextBox ID="txtEscONB_ALL_MONEY" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label text-blue" runat="server" Text="附記："></asp:Label>
                                                                <asp:TextBox ID="txtEscOVC_MARK" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="承辦單位統一編號："></asp:Label>
                                                                <asp:TextBox ID="txtEscOVC_WORK_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                                <asp:Label CssClass="control-label" runat="server" Text="全銜"></asp:Label>
                                                                <asp:TextBox ID="txtEscOVC_WORK_NAME" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="業務部門名稱："></asp:Label>
                                                                <asp:TextBox ID="txtEscOVC_WORK_UNIT" CssClass="tb tb-l" Text="採購發包處" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="財務收繳單位："></asp:Label>
                                                                <asp:DropDownList ID="drpEscOVC_ONNAME" OnSelectedIndexChanged="drpEscOVC_ONNAME_SelectedIndexChanged" AutoPostBack="true" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                                                <asp:TextBox ID="txtEscOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label position-left" runat="server" Text="收入單通知編號："></asp:Label>
                                                                <asp:TextBox ID="txtEscOVC_RECEIVE_NO" CssClass="position-left tb tb-l" runat="server"></asp:TextBox>
                                                                <asp:Label CssClass="control-label position-left" runat="server"> 日期：</asp:Label>
                                                                <div class="input-append datepicker">
                                                                    <asp:TextBox ID="txtEscOVC_DRECEIVE" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                                    <span class='add-on'><i class="icon-calendar"></i></span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label position-left" runat="server" Text="財務單位編號："></asp:Label>
                                                                <asp:TextBox ID="txtEscOVC_COMPTROLLER_NO" CssClass="position-left tb tb-l" runat="server"></asp:TextBox>
                                                                <asp:Label CssClass="control-label position-left" runat="server"> 收訖日期：</asp:Label>
                                                                <div class="input-append datepicker">
                                                                    <asp:TextBox ID="txtEscOVC_DCOMPTROLLER" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                                    <span class='add-on'><i class="icon-calendar"></i></span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table class="table table-bordered text-center">
                                                        <tr class="screentone-gray">
                                                            <td style="width: 50%" class="text-right">
                                                                <asp:Label CssClass="control-label text-red" runat="server">主官核批日：</asp:Label></td>
                                                            <td>

                                                                <div class="input-append datepicker">
                                                                    <asp:TextBox ID="txtEscOVC_DAPPROVE" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                                    <span class='add-on'><i class="icon-calendar"></i></span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" class="screentone-gray">
                                                                <asp:Label CssClass="control-label position-left text-blue" runat="server" Text="註1：已經存檔的[主管核批日](用來計算作業天數及在未決標的特例下進入下一階段)用戶無法於前端介面刪除！"></asp:Label>
                                                                <asp:Label CssClass="control-label position-left text-blue" runat="server" Text="註2：因此，若全案尚須回[保證金收據]之前的流程作業(考慮分組)，請直接[存檔]即可，先不要輸入[主管核批日]！"></asp:Label>
                                                                <asp:Label CssClass="control-label position-left text-blue" runat="server" Text="註3：請確定全案不需要再回到[開標結果](含)之前之各階段作業，再輸入[主管核批日]，然後存檔！"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div style="text-align: center">
                                                        <asp:Button ID="btnSaveC" OnClick="btnSaveC_Click" CssClass="btn-default btnw2" runat="server" Text="存檔" />
                                                        <asp:Button ID="btnToC" OnClick="btnToC_Click" CssClass="btn-default" runat="server" Text="回保證金(函)選擇畫面" />
                                                        <asp:LinkButton ID="lnkC" OnClick="lnkC_Click" runat="server">收入通知單預覽列印.doc</asp:LinkButton>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="lnkC_odt" OnClick="lnkC_odt_Click" runat="server">收入通知單預覽列印.odt</asp:LinkButton>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="lnkC" />
                                                <asp:PostBackTrigger ControlID="btnCashNo" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div id="TabDoc" class="tab-pane ">
                                        <!--代管保證文件-->
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:Panel ID="PnMessageD" runat="server"></asp:Panel>
                                                <asp:Panel ID="panDocD" runat="server">
                                                    <asp:GridView ID="GV_TBMMANAGE_PROM" DataKeyNames="OVC_PURCH_6" CssClass="table data-table table-striped border-top text-center" OnPreRender="GV_TBMMANAGE_PROM_PreRender" OnRowCommand="GV_TBMMANAGE_PROM_RowCommand" AutoGenerateColumns="false" runat="server">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="作業">
                                                                <ItemTemplate>
                                                                    <asp:Button CssClass="btn-default" Text="異動" CommandName="btnwork" runat="server" />
                                                                    <asp:Button CssClass="btn-warning" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" Visible='<%#  Eval("OVC_DAPPROVE").ToString() != "" ? false:true %>' Text="刪除" CommandName="btnDel" runat="server" /><br />
                                                                    <div visible='<%# Container.DisplayIndex + 1 == 1 ? false : true %>' runat="server">
                                                                        由項次:<asp:TextBox ID="txtCopy" CssClass="tb tb-s" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                                                        <asp:Button ID="btnCopy" CommandName="Copy" Text="複製" CssClass="btn-default" runat="server"></asp:Button>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="項次">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItem" CssClass="control-label" Text='<%#Container.DataItemIndex+1 %>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="類別" DataField="OVC_KIND" />
                                                            <asp:TemplateField HeaderText="合約編號">
                                                                <ItemTemplate>
                                                                    <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PURCH") %>' runat="server"></asp:Label>
                                                                    <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PUR_AGENCY") %>' runat="server"></asp:Label>
                                                                    <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PURCH_5") %>' runat="server"></asp:Label>
                                                                    <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PURCH_6") %>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="款項所有權人" DataField="OVC_OWN_NAME" />
                                                            <asp:BoundField HeaderText="合計金額" DataField="ONB_ALL_MONEY" />
                                                            <asp:BoundField HeaderText="收入單通知編號" DataField="OVC_RECEIVE_NO" />
                                                            <asp:BoundField HeaderText="收入日期" DataField="OVC_DRECEIVE" />
                                                        </Columns>
                                                    </asp:GridView>
                                                    <div style="text-align: center">
                                                        <asp:Button ID="btnNewD" OnClick="btnNewD_Click" CssClass="btn-default btnw2" runat="server" Text="新增" />
                                                        <br /><br />
                                                        <asp:Label ID="lblPromNo" CssClass="control-label" runat="server" Text="由編號："></asp:Label>
                                                        <asp:TextBox ID="txtPromNo" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                        <asp:Button ID="btnPromNo" Text="複製" OnClick="btnPromNo_Click" CssClass="btn-default btnw2" runat="server" />
                                                        <br /><br />
                                                        <asp:Label ID="lblCashNo_P" CssClass="control-label" runat="server" Text="由代管現金編號："></asp:Label>
                                                        <asp:TextBox ID="txtCashNo_P" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                        <asp:Button ID="btnCashNo_P" Text="複製" OnClick="btnCashNo_P_Click" CssClass="btn-default btnw2" runat="server" />
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel ID="panDoc" runat="server">
                                                    <table class="table table-bordered">
                                                        <tr>
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="購案合約編號："></asp:Label>
                                                                <asp:Label ID="lblDocOVC_PURCH_6" CssClass="control-label" runat="server"></asp:Label>
                                                                <asp:TextBox CssClass="tb tb-s" ID="txtDocOVC_PURCH_6" runat="server"></asp:TextBox>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:LinkButton ID="LinkButton2" OnClientClick="OpenQC(2)" runat="server">查詢本合約之合約商資料</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="保證人："></asp:Label>
                                                                <asp:TextBox ID="txtDocOVC_OWN_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                                <%--<asp:Label CssClass="control-label" runat="server" Text="保證人統一編號："></asp:Label>
                                                        <asp:TextBox ID="txtDocOVC_OWN_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>--%>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="保證人地址："></asp:Label>
                                                                <asp:TextBox ID="txtDocOVC_OWN_ADDRESS" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="被保證人："></asp:Label>
                                                                <asp:TextBox ID="txtDocOVC_OWNED_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Label CssClass="control-label" runat="server" Text="被保證人統一編號："></asp:Label>
                                                                <asp:TextBox ID="txtDocOVC_OWNED_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="被保證人地址："></asp:Label>
                                                                <asp:TextBox ID="txtDocOVC_OWNED_ADDRESS" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td class="td-inner-table">
                                                                <asp:Label CssClass="control-label text-blue" runat="server" Text="代管項目"></asp:Label>
                                                                <asp:Label CssClass="control-label text-red" runat="server" Text="(最多三筆)"></asp:Label>
                                                                <table class="table table-inner table-bordered text-center">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label CssClass="control-label text-green" runat="server" Text="項次"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label CssClass="control-label text-green" runat="server" Text="代管事由說明"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label CssClass="control-label text-green" runat="server" Text="保證文件名稱"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label CssClass="control-label text-green" runat="server" Text="保證文件份數"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label CssClass="control-label text-green" runat="server" Text="入帳金額(必填)"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label CssClass="control-label text-green" runat="server" Text="有效日期"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblDocONB_ITEM_1" CssClass="control-label text-green" runat="server" Text="1"></asp:Label></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDocOVC_REASON_1" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDocOVC_NSTOCK_1" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDocOVC_NUMBER_1" CssClass="tb tb-xs" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDocONB_MONEY_1" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                        <td>

                                                                            <div class="input-append datepicker">
                                                                                <asp:TextBox ID="txtDocExpDate_1" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                                                <span class='add-on'><i class="icon-calendar"></i></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblDocONB_ITEM_2" CssClass="control-label text-green" runat="server" Text="2"></asp:Label></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDocOVC_REASON_2" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDocOVC_NSTOCK_2" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDocOVC_NUMBER_2" CssClass="tb tb-xs" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDocONB_MONEY_2" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                        <td>

                                                                            <div class="input-append datepicker">
                                                                                <asp:TextBox ID="txtDocExpDate_2" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                                                <span class='add-on'><i class="icon-calendar"></i></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblDocONB_ITEM_3" CssClass="control-label text-green" runat="server" Text="3"></asp:Label></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDocOVC_REASON_3" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDocOVC_NSTOCK_3" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDocOVC_NUMBER_3" CssClass="tb tb-xs" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDocONB_MONEY_3" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                        <td>

                                                                            <div class="input-append datepicker">
                                                                                <asp:TextBox ID="txtDocExpDate_3" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                                                <span class='add-on'><i class="icon-calendar"></i></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:Label CssClass="control-label text-red" runat="server" Text="合計金額："></asp:Label>
                                                                <asp:TextBox ID="txtDocONB_ALL_MONEY" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label text-blue" runat="server" Text="附記："></asp:Label>
                                                                <asp:TextBox ID="txtDocOVC_MARK" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="承辦單位統一編號："></asp:Label>
                                                                <asp:TextBox ID="txtDocOVC_WORK_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                                <asp:Label CssClass="control-label" runat="server" Text="全銜"></asp:Label>
                                                                <asp:TextBox ID="txtDocOVC_WORK_NAME" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="業務部門名稱："></asp:Label>
                                                                <asp:TextBox ID="txtDocOVC_WORK_UNIT" CssClass="tb tb-l" Text="採購發包處" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="財務收繳單位："></asp:Label>
                                                                <asp:DropDownList ID="drpDocOVC_ONNAME" OnSelectedIndexChanged="drpDocOVC_ONNAME_SelectedIndexChanged" AutoPostBack="true" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                                                <asp:TextBox ID="txtDocOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label position-left" runat="server" Text="收入單通知編號："></asp:Label>
                                                                <asp:TextBox ID="txtDocOVC_RECEIVE_NO" CssClass="position-left tb tb-l" runat="server"></asp:TextBox>
                                                                <asp:Label CssClass="control-label position-left" runat="server">日期：</asp:Label>

                                                                <div class="input-append datepicker">
                                                                    <asp:TextBox ID="txtDocOVC_DRECEIVE" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                                    <span class='add-on'><i class="icon-calendar"></i></span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label position-left" runat="server" Text="財務單位編號："></asp:Label>
                                                                <asp:TextBox ID="txtDocOVC_COMPTROLLER_NO" CssClass="position-left tb tb-l" runat="server"></asp:TextBox>
                                                                <asp:Label CssClass="control-label position-left" runat="server">收訖日期：</asp:Label>

                                                                <div class="input-append datepicker">
                                                                    <asp:TextBox ID="txtDocOVC_DCOMPTROLLER" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                                    <span class='add-on'><i class="icon-calendar"></i></span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table class="table table-bordered text-center">
                                                        <tr class="screentone-gray">
                                                            <td style="width: 50%" class="text-right">
                                                                <asp:Label CssClass="control-label text-red" runat="server">主官核批日：</asp:Label></td>
                                                            <td>

                                                                <div class="input-append datepicker">
                                                                    <asp:TextBox ID="txtDocOVC_DAPPROVE" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                                    <span class='add-on'><i class="icon-calendar"></i></span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" class="screentone-gray">
                                                                <asp:Label CssClass="control-label position-left text-blue" runat="server" Text="註1：已經存檔的[主管核批日](用來計算作業天數及在未決標的特例下進入下一階段)用戶無法於前端介面刪除！"></asp:Label>
                                                                <asp:Label CssClass="control-label position-left text-blue" runat="server" Text="註2：因此，若全案尚須回[保證金收據]之前的流程作業(考慮分組)，請直接[存檔]即可，先不要輸入[主管核批日]！"></asp:Label>
                                                                <asp:Label CssClass="control-label position-left text-blue" runat="server" Text="註3：請確定全案不需要再回到[開標結果](含)之前之各階段作業，再輸入[主管核批日]，然後存檔！"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div style="text-align: center">
                                                        <asp:Button ID="btnSaveD" OnClick="btnSaveD_Click" CssClass="btn-default btnw2" runat="server" Text="存檔" />
                                                        <asp:Button ID="btnToD" OnClick="btnToD_Click" CssClass="btn-default" runat="server" Text="回保證金(函)選擇畫面" />
                                                    </div>
                                                    <br />
                                                    <div style="text-align: center">
                                                        <asp:LinkButton ID="lnkD" OnClick="lnkD_Click" runat="server">收入通知單預覽列印.doc</asp:LinkButton>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="lbtnJointGuarantee" OnClick="lbtnJointGuarantee_Click" runat="server">列印連帶保證書.doc</asp:LinkButton>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="lbtnFax" OnClick="lbtnFax_Click" runat="server">傳真使用申請表.doc</asp:LinkButton>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="lbtnTelFax" OnClick="lbtnTelFax_Click" runat="server">電話傳真表.doc</asp:LinkButton>
                                                        <br />
                                                        <asp:LinkButton ID="lnkD_odt" OnClick="lnkD_odt_Click" runat="server">收入通知單預覽列印.odt</asp:LinkButton>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="lbtnJointGuarantee_odt" OnClick="lbtnJointGuarantee_odt_Click" runat="server">列印連帶保證書.odt</asp:LinkButton>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="lbtnFax_odt" OnClick="lbtnFax_odt_Click" runat="server">傳真使用申請表.odt</asp:LinkButton>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="lbtnTelFax_odt" OnClick="lbtnTelFax_odt_Click" runat="server">電話傳真表.odt</asp:LinkButton>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="lnkD" />
                                                <asp:PostBackTrigger ControlID="lbtnJointGuarantee" />
                                                <asp:PostBackTrigger ControlID="lbtnFax" />
                                                <asp:PostBackTrigger ControlID="lbtnTelFax" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div id="TabSec" class="tab-pane">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <!--代管有價證券-->
                                                <asp:Panel ID="PnMessageS" runat="server"></asp:Panel>
                                                <asp:Panel ID="panSecD" runat="server">
                                                    <asp:GridView ID="GV_TBMMANAGE_STOCK" DataKeyNames="OVC_PURCH_6" CssClass="table data-table table-striped border-top text-center" OnPreRender="GV_TBMMANAGE_STOCK_PreRender" OnRowCommand="GV_TBMMANAGE_STOCK_RowCommand" AutoGenerateColumns="false" runat="server">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="作業">
                                                                <ItemTemplate>
                                                                    <asp:Button CssClass="btn-default" Text="異動" CommandName="btnwork" runat="server" />
                                                                    <asp:Button CssClass="btn-warning" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" Visible='<%#  Eval("OVC_DAPPROVE").ToString() != "" ? false:true %>' Text="刪除" CommandName="btnDel" runat="server" />
                                                                    <div visible='<%# Container.DisplayIndex + 1 == 1 ? false : true %>' runat="server">
                                                                        由項次:<asp:TextBox ID="txtCopy" CssClass="tb tb-s" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}" runat="server"></asp:TextBox>
                                                                        <asp:Button ID="btnCopy" CommandName="Copy" Text="複製" CssClass="btn-default" runat="server"></asp:Button>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="項次">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItem" CssClass="control-label" Text='<%#Container.DataItemIndex+1 %>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="類別" DataField="OVC_KIND" />
                                                            <asp:TemplateField HeaderText="合約編號">
                                                                <ItemTemplate>
                                                                    <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PURCH") %>' runat="server"></asp:Label>
                                                                    <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PUR_AGENCY") %>' runat="server"></asp:Label>
                                                                    <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PURCH_5") %>' runat="server"></asp:Label>
                                                                    <asp:Label CssClass="control-label" Text='<%# Eval("OVC_PURCH_6") %>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="款項所有權人" DataField="OVC_OWN_NAME" />
                                                            <asp:BoundField HeaderText="合計金額" DataField="ONB_ALL_MONEY" />
                                                            <asp:BoundField HeaderText="收入單通知編號" DataField="OVC_RECEIVE_NO" />
                                                            <asp:BoundField HeaderText="收入日期" DataField="OVC_DRECEIVE" />
                                                        </Columns>
                                                    </asp:GridView>
                                                    <div style="text-align: center">
                                                        <asp:Button ID="btnNewS" OnClick="btnNewS_Click" CssClass="btn-default btnw2" runat="server" Text="新增" />
                                                        <br /><br />
                                                        <asp:Label ID="lblStockNo" CssClass="control-label" runat="server" Text="由編號："></asp:Label>
                                                        <asp:TextBox ID="txtStockNo" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                        <asp:Button ID="btnStockNo" Text="複製" OnClick="btnStockNo_Click" CssClass="btn-default btnw2" runat="server" />
                                                        <br /><br />
                                                        <asp:Label ID="lblCashNo_S" CssClass="control-label" runat="server" Text="由代管現金編號："></asp:Label>
                                                        <asp:TextBox ID="txtCashNo_S" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                                        <asp:Button ID="btnCashNo_S" Text="複製" CssClass="btn-default btnw2" OnClick="btnCashNo_S_Click" runat="server" />
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel ID="panSec" runat="server">
                                                    <table class="table table-bordered">
                                                        <tr>
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="購案合約編號："></asp:Label>
                                                                <asp:Label ID="lblSecOVC_PURCH_6" CssClass="control-label" runat="server"></asp:Label>
                                                                <asp:TextBox CssClass="tb tb-s" ID="txtSecOVC_PURCH_6" runat="server"></asp:TextBox>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:LinkButton ID="LinkButton3" OnClientClick="OpenQC(3)" runat="server">查詢本合約之合約商資料</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="有價證券所有權人："></asp:Label>
                                                                <asp:TextBox ID="txtSecOVC_OWN_NAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                                <asp:Label CssClass="control-label" runat="server" Text="有價證券所有權人統一編號："></asp:Label>
                                                                <asp:TextBox ID="txtSecOVC_OWN_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="有價證券所有權人地址："></asp:Label>
                                                                <asp:TextBox ID="txtSecOVC_OWN_ADDRESS" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="有價證券所有權人電話："></asp:Label>
                                                                <asp:TextBox ID="txtSecOVC_OWN_TEL" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td class="td-inner-table">
                                                                <asp:Label CssClass="control-label text-blue" runat="server" Text="代管項目"></asp:Label>
                                                                <asp:Label CssClass="control-label text-red" runat="server" Text="(最多三筆)"></asp:Label>
                                                                <table class="table table-inner table-bordered text-center">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label CssClass="control-label text-green" runat="server" Text="項次"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label CssClass="control-label text-green" runat="server" Text="代管事由說明"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label CssClass="control-label text-green" runat="server" Text="有價證券名稱"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label CssClass="control-label text-green" runat="server" Text="有價證券面額衡量單位"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label CssClass="control-label text-green" runat="server" Text="有價證券面額數量"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label CssClass="control-label text-green" runat="server" Text="新台幣入帳金額(必填)"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblSecONB_ITEM_1" CssClass="control-label text-green" runat="server" Text="1"></asp:Label></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSecOVC_REASON_1" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSecOVC_NSTOCK_1" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSecOVC_CURRENT_1" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSecONB_MONEY_1" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSecONB_MONEY_NT_1" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblSecONB_ITEM_2" CssClass="control-label text-green" runat="server" Text="2"></asp:Label></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSecOVC_REASON_2" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSecOVC_NSTOCK_2" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSecOVC_CURRENT_2" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSecONB_MONEY_2" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSecONB_MONEY_NT_2" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblSecONB_ITEM_3" CssClass="control-label text-green" runat="server" Text="3"></asp:Label></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSecOVC_REASON_3" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSecOVC_NSTOCK_3" CssClass="tb tb-m" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSecOVC_CURRENT_3" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSecONB_MONEY_3" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSecONB_MONEY_NT_3" CssClass="tb tb-s" runat="server"></asp:TextBox></td>
                                                                    </tr>
                                                                </table>
                                                                <asp:Label CssClass="control-label text-red" runat="server" Text="合計金額："></asp:Label>
                                                                <asp:TextBox ID="txtSecONB_ALL_MONEY" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label text-blue" runat="server" Text="附記："></asp:Label>
                                                                <asp:TextBox ID="txtSecOVC_MARK" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="承辦單位統一編號："></asp:Label>
                                                                <asp:TextBox ID="txtSecOVC_WORK_NO" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                                <asp:Label CssClass="control-label" runat="server" Text="全銜"></asp:Label>
                                                                <asp:TextBox ID="txtSecOVC_WORK_NAME" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="業務部門名稱："></asp:Label>
                                                                <asp:TextBox ID="txtSecOVC_WORK_UNIT" CssClass="tb tb-l" Text="採購發包處" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label" runat="server" Text="財務收繳單位："></asp:Label>
                                                                <asp:DropDownList ID="drpSecOVC_ONNAME" OnSelectedIndexChanged="drpSecOVC_ONNAME_SelectedIndexChanged" AutoPostBack="true" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                                                <asp:TextBox ID="txtSecOVC_ONNAME" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label position-left" runat="server" Text="收入單通知編號："></asp:Label>
                                                                <asp:TextBox ID="txtSecOVC_RECEIVE_NO" CssClass="position-left tb tb-l" runat="server"></asp:TextBox>
                                                                <asp:Label CssClass="control-label position-left" runat="server">日期：</asp:Label>

                                                                <div class="input-append datepicker">
                                                                    <asp:TextBox ID="txtSecOVC_DRECEIVE" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                                    <span class='add-on'><i class="icon-calendar"></i></span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr class="no-bordered-seesaw">
                                                            <td>
                                                                <asp:Label CssClass="control-label position-left" runat="server" Text="財務單位編號："></asp:Label>
                                                                <asp:TextBox ID="txtSecOVC_COMPTROLLER_NO" CssClass="position-left tb tb-l" runat="server"></asp:TextBox>
                                                                <asp:Label CssClass="control-label position-left" runat="server">收訖日期：</asp:Label>

                                                                <div class="input-append datepicker">
                                                                    <asp:TextBox ID="txtSecOVC_DCOMPTROLLER" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                                    <span class='add-on'><i class="icon-calendar"></i></span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table class="table table-bordered text-center">
                                                        <tr class="screentone-gray">
                                                            <td style="width: 50%" class="text-right">
                                                                <asp:Label CssClass="control-label text-red" runat="server">主官核批日：</asp:Label></td>
                                                            <td>

                                                                <div class="input-append datepicker">
                                                                    <asp:TextBox ID="txtSecOVC_DAPPROVE" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                                    <span class='add-on'><i class="icon-calendar"></i></span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" class="screentone-gray">
                                                                <asp:Label CssClass="control-label position-left text-blue" runat="server" Text="註1：已經存檔的[主管核批日](用來計算作業天數及在未決標的特例下進入下一階段)用戶無法於前端介面刪除！"></asp:Label>
                                                                <asp:Label CssClass="control-label position-left text-blue" runat="server" Text="註2：因此，若全案尚須回[保證金收據]之前的流程作業(考慮分組)，請直接[存檔]即可，先不要輸入[主管核批日]！"></asp:Label>
                                                                <asp:Label CssClass="control-label position-left text-blue" runat="server" Text="註3：請確定全案不需要再回到[開標結果](含)之前之各階段作業，再輸入[主管核批日]，然後存檔！"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div style="text-align: center">
                                                        <asp:Button ID="btnSaveS" OnClick="btnSaveS_Click" CssClass="btn-default btnw2" runat="server" Text="存檔" />
                                                        <asp:Button ID="btnToS" OnClick="btnToS_Click" CssClass="btn-default" runat="server" Text="回保證金(函)選擇畫面" />
                                                        <asp:LinkButton ID="lnkS" OnClick="lnkS_Click" runat="server">收入通知單預覽列印.doc</asp:LinkButton>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="lnkS_odt" OnClick="lnkS_odt_Click" runat="server">收入通知單預覽列印.odt</asp:LinkButton>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="lnkS" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        
                                    </div>
                                </div>
                            </div>
                            <!--頁籤－結束-->
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

                            <div style="text-align: center">
                                <asp:Button ID="btnContract" CssClass="btn-default" Text="回簽約畫面" OnClick="btnContract_Click" runat="server" />&nbsp;&nbsp;
                                <asp:Button ID="btnSave" CssClass="btn-default" Text ="存檔" OnClick="btnSave_Click" runat="server" />&nbsp;&nbsp;
                                <asp:Button ID="btnMain" OnClick="btnMain_Click" CssClass="btn-default" runat="server" Text="回主流程畫面" />
                            </div>
                            <br />
                            <br />
                            </asp:Panel>


                            <div>
                                <table class="table table-bordered text-center">
                                    <tr>
                                        <td colspan="2"><div class="subtitle">採購發包階段上傳檔案 </div></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="td-inner-table">
                                            <asp:GridView ID="gvFiles" CssClass="table data-table table-striped border-top text-center table-inner" Width="100%" AutoGenerateColumns="False" runat="server" >
 		     		                            <Columns>
                                                    <asp:HyperLinkField DataTextField="FileName" HeaderText="上傳之檔案名稱" />
                                                    <asp:BoundField DataField="Time" HeaderText="時間" />     
                                                      <asp:TemplateField HeaderText="檔案大小">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Eval("FileSize") %>' runat="server"></asp:Label> KB
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                	                            </Columns>
	   		                                </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><asp:Button ID="btnUpload" CssClass="btn-default btnw2" Text="上傳" OnClick="btnUpload_Click" runat="server"/></td>
                                        <td style="width:80%">
                                            <asp:FileUpload ID="FileUpload" title="瀏覽..." runat="server" /></td>
                                    </tr>
                                </table>
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
