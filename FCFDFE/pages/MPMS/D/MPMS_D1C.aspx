<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_D1C.aspx.cs" Inherits="FCFDFE.pages.MPMS.D.MPMS_D1C" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("<%=strMenuName%>").addClass("active");
            $("<%=strMenuNameItem%>").addClass("active");
        });
    </script>
    <script>
        function OpenD1C1() {
            var win_width = 1200;
            var win_height = 1000;
            var PosX = (screen.width - win_width) / 2;
            var PosY = (screen.Height - win_height) / 2;
            features = "width=" + win_width + ",height=" + win_height + ",top=" + PosY + ",left=" + PosX;
            var OVC_PURCH = '<%=sOVC_PURCH%>';
            var ONB_GROUP = '<%=sONB_GROUP%>';
            var theURL = '<%=ResolveClientUrl("MPMS_D1C_1.aspx?OVC_PURCH=")%>' + OVC_PURCH  + '&ONB_GROUP=' +ONB_GRUOP;
            var newwin = window.open(theURL, 'unitQuery', features);
        }
    </script>
    <script>
        function OpenD351() {
            var win_width = 1200;
            var win_height = 1000;
            var PosX = (screen.width - win_width) / 2;
            var PosY = (screen.Height - win_height) / 2;
            features = "width=" + win_width + ",height=" + win_height + ",top=" + PosY + ",left=" + PosX;
            var OVC_PURCH = '<%=sOVC_PURCH%>';
            var ONB_GROUP = '<%=sONB_GROUP%>';
            var theURL = '<%=ResolveClientUrl("MPMS_D1C_1.aspx?OVC_PURCH=")%>' + OVC_PURCH + '&ONB_GROUP=' + ONB_GROUP;
            var newwin = window.open(theURL, 'unitQuery', features);
        }
    </script>

    <div class="row">
        <asp:Panel ID="PnMessage_Top" runat="server"></asp:Panel>
        <div style="width: 1000px; margin: auto;" id="divForm" visible="false" runat="server">
            <asp:Panel ID="Panel1" runat="server">
                <section class="panel">
                    <header class="title">
                        <!--標題-->
                        <asp:Label ID="lblPURCH" CssClass="control-label" runat="server"></asp:Label>
                        <asp:Label ID="lblTitle" CssClass="control-label" runat="server">購案決標公告作業</asp:Label>
                    </header>
                    <asp:Panel ID="Panel3" runat="server"></asp:Panel>
                    <!--預留空間，未來做錯誤訊息顯示。-->
                    <div class="panel-body" style="border: solid 2px;">
                        <div class="form" style="border: 5px;">
                            <div class="cmxform form-horizontal tasi-form">
                                <!--網頁內容-->
                                <asp:GridView ID="GV_TBMRESULT_ANNOUNCE" CssClass="table table-striped border-top text-center" DataKeyNames="OVC_PURCH_6" OnPreRender="GV_TBMRESULT_ANNOUNCE_PreRender" OnRowCommand="GV_TBMRESULT_ANNOUNCE_RowCommand" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:TemplateField HeaderText="作業">
                                            <ItemTemplate>
                                                <asp:Button CssClass="btn-default" ID="btnWork" Text="異動" CommandName="btnWork" runat="server" />
                                                <asp:Button CssClass="btn-warning" ID="btnDel" OnClientClick="if (confirm('確定刪除此資料?') == false) return false;" Text="刪除"  CommandName="btnDel" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="購案編號">
                                            <ItemTemplate>
                                                <asp:Label  CssClass="control-label" Text='<%# Eval("OVC_PURCH") %>' runat="server"></asp:Label>
                                                <asp:Label  CssClass="control-label" Text='<%# Eval("OVC_PUR_AGENCY") %>' runat="server"></asp:Label>
                                                <asp:Label  CssClass="control-label" Text='<%# Eval("OVC_PURCH_5") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="契約編號" DataField="OVC_PURCH_6" />
                                        <asp:BoundField HeaderText="組別" DataField="ONB_GROUP" />
                                        <asp:BoundField HeaderText="決標日期" DataField="OVC_DBID" />
                                        <asp:BoundField HeaderText="資料傳輸日" DataField="OVC_DSEND" />
                                        <asp:BoundField HeaderText="承辦人" DataField="OVC_NAME" />
                                    </Columns>
                                </asp:GridView>
                                <div style="text-align: center">
                                    <asp:Button CssClass="btn-default" OnClick="btnBefore_Click" ID="btnBefore" Text="回開標結果作業" runat="server" />
                                    <asp:Button CssClass="btn-default" OnClick="btnMain_Click" ID="btnMain" Text="回主流程畫面" runat="server" />
                                </div>
                            </div>
                        </div>

                    </div>
                    <footer class="panel-footer" style="text-align: center;">
                        <!--網頁尾-->
                    </footer>
                </section>
            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server">
                <section class="panel">
                    <header class="title">
                        <!--標題-->
                        決標公告(一般)編輯
                    </header>
                    <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                    <!--預留空間，未來做錯誤訊息顯示。-->
                    <div class="panel-body" style="border: solid 2px;">
                        <div class="form" style="border: 5px;">
                            <div class="cmxform form-horizontal tasi-form">
                                <!--網頁內容-->
                                <table class="table no-bordered-seesaw text-center">
                                    <tr class="no-bordered-seesaw">
                                        <td style="width: 40%" class="text-right no-bordered">
                                            <asp:Label CssClass="control-label" runat="server">傳輸日期 :</asp:Label></td>
                                        <td class="no-bordered">
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtDSEND" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                <span class='add-on'><i class="icon-calendar"></i></span><asp:Label CssClass="control-label position-left" runat="server">(上一次傳輸日期 : </asp:Label>
                                    <asp:Label ID="lblDSEND" CssClass="control-label position-left" runat="server">)</asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                
                                <table class="table table-bordered">
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="契約編號："></asp:Label>
                                            <!--購案編號?契約編號?-->
                                            <asp:Label ID="lblOVC_PURCH" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="標的名稱："></asp:Label>
                                            <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="部分決標項目或數量："></asp:Label>
                                            <asp:TextBox ID="txtOVC_PART_IPURCH" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="招標方式："></asp:Label>
                                            <asp:RadioButtonList ID="rdoOVC_PUR_ASS_VEN_CODE" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                <asp:ListItem Value="A">公開招標</asp:ListItem>
                                                <asp:ListItem Value="B">選擇性招標</asp:ListItem>
                                                <asp:ListItem Value="C">限制性招標公告徵求</asp:ListItem>
                                                <asp:ListItem Value="D">限制性招標公開評選</asp:ListItem>
                                                <asp:ListItem Value="E">公開取得廠商報價或企畫書</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="執行現況："></asp:Label>
                                            <asp:RadioButtonList ID="rOVC_STATUS" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                <asp:ListItem Value="Y">已決標</asp:ListItem>
                                                <asp:ListItem Value="N">部分決標</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="決標日期："></asp:Label>
                                            <asp:Label ID="lblOVC_DBID" CssClass="control-label" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="預算金額："></asp:Label>
                                            <asp:Label ID="lblOVC_CURRENT" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtONB_BUDGET" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="底價金額或評審委員會建議金額："></asp:Label>
                                            <asp:Label ID="lblOVC_BID_CURRENT" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtONB_BID_BUDGET" CssClass="tb tb-m" runat="server"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="總決標金額："></asp:Label>
                                            <asp:Label ID="lblOVC_RESULT_CURRENT" CssClass="control-label" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtONB_BID_RESULT" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="投標廠商家數："></asp:Label>
                                            <asp:Label ID="lblONB_BID_VENDORS" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="得標廠商家數："></asp:Label>
                                            <asp:Label ID="lblONB_RESULT_VENDORS" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="未得標廠商代碼及名稱："></asp:Label>
                                            <asp:TextBox ID="txtOVC_NONE_VENDORS" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <asp:Label CssClass="control-label text-blue subtitle" runat="server" Text="得標廠商資料" Font-Bold="True"></asp:Label>
                                </div>
                                <table class="table table-bordered">
                                    <tr>
                                        <td class="text-center">
                                            <asp:Label ID="lblOVC_VEN_TITLE" CssClass="control-label text-red" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="廠商代碼："></asp:Label>
                                            <asp:Label ID="lblOVC_VEN_CST" CssClass="control-label" runat="server"></asp:Label>
                                            <!--找不到-->
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="廠商名稱："></asp:Label>
                                            <asp:Label ID="lblOVC_VEN_TITLE_1" CssClass="control-label" runat="server"></asp:Label>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="廠商地址："></asp:Label>
                                            <asp:TextBox ID="txtOVC_VEN_ADDRESS" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="廠商電話："></asp:Label>
                                            <asp:TextBox ID="txtOVC_VEN_TEL" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="雇用員工總人數是否超過一百人："></asp:Label><br>
                                            <asp:RadioButton AutoPostBack="true" ID="rdoOVC_EMPLOYEE_OVER_Y" OnCheckedChanged="rdoOVC_EMPLOYEE_OVER_Y_CheckedChanged" CssClass="radioButton" runat="server" Text="是" />
                                            <asp:Label CssClass="control-label" runat="server" Text="：雇用員工總人數"></asp:Label>
                                            <asp:TextBox ID="ONB_EMPLOYEES" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                            <asp:Label CssClass="control-label" runat="server" Text="人，已僱用身心障礙人士"></asp:Label>
                                            <asp:TextBox ID="txtONB_EMPLOYEES_SPECIAL" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                            <asp:Label CssClass="control-label" runat="server" Text="人，已僱用原住民人士"></asp:Label>
                                            <asp:TextBox ID="txtONB_EMPLOYEES_ABORIGINAL" CssClass="tb tb-xs" runat="server"></asp:TextBox>
                                            <asp:Label CssClass="control-label" runat="server" Text="人。"></asp:Label><br>
                                            <asp:RadioButton AutoPostBack="true" ID="rdoOVC_EMPLOYEE_OVER_N" OnCheckedChanged="rdoOVC_EMPLOYEE_OVER_N_CheckedChanged" CssClass="radioButton" runat="server" Text="否" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="得標商類別："></asp:Label><br>
                                            <!--組別-->
                                            <asp:RadioButtonList ID="rdoVEN_KIND" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                <asp:ListItem Value="營造業">1.營造業</asp:ListItem>
                                                <asp:ListItem Value="技師事務所">2.技師事務所</asp:ListItem>
                                                <asp:ListItem Value="技師顧問機構">3.技術顧問機構</asp:ListItem>
                                                <asp:ListItem Value="建築事務所">4.建築事務所</asp:ListItem>
                                                <asp:ListItem Value="其他">5.其他</asp:ListItem>
                                            </asp:RadioButtonList>
                                            <asp:TextBox ID="txtOthers" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="併列共同供應商個別廠商之決標金額："></asp:Label>
                                            <asp:TextBox ID="txtONB_BID_RESULT_MERG" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="是否為中小企業："></asp:Label>
                                            <asp:RadioButtonList ID="rdoOVC_MIDDLE_SMALL" CssClass="radioButton" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                <asp:ListItem Value="Y">是</asp:ListItem>
                                                <asp:ListItem Value="N">否</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="決標金額："></asp:Label>
                                            <asp:TextBox ID="txtONB_BID_RESULT_1" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="預估分包予中小企業之金額："></asp:Label>
                                            <asp:TextBox ID="txtONB_BID_JOB" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="原產地國別或得標廠商國別："></asp:Label>
                                            <asp:DropDownList ID="drpOVC_VEN_COUNTRY" CssClass="tb tb-m" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="#得標廠商有採購法第103條第2項規定情形之上級機關核准文號："></asp:Label>
                                            <asp:TextBox ID="txtOVC_VEN_103_2" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                            <asp:Label ID="lblOVC_BID_METHOD_1" CssClass="control-label" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="control-label" runat="server" Text="[附加說明]"></asp:Label>
                                            <asp:TextBox ID="txtOVC_DESC" CssClass="tb tb-full" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table no-bordered-seesaw text-center">
                                    <tr class="no-bordered-seesaw">
                                        <td style="width: 50%" class="text-right no-bordered">
                                            <asp:Label CssClass="control-label text-red" runat="server">主官核批日：</asp:Label></td>
                                        <td class="no-bordered">
                                            <div class="input-append datepicker">
                                                <asp:TextBox ID="txtOVC_DAPPROVE" CssClass="tb tb-s position-left text-change" AutoPostBack="true" runat="server"></asp:TextBox>
                                                <span class='add-on'><i class="icon-calendar"></i></span>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div style="text-align: center">
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click"  CssClass="btn-default btnw2" runat="server" Text="存檔" /><br /><br/>
                                    <asp:Button ID="btnNext" OnClick="btnNext_Click" CssClass="btn-default" runat="server" Text="回上一頁" />
                                    <asp:Button ID="btnEdit" OnClick="btnEdit_Click"  CssClass="btn-default" runat="server" Text="回採購開標結果作業畫面" />
                                    <asp:Button ID="btnToMain" OnClick="btnToMain_Click"  CssClass="btn-default" runat="server" Text="回主流程畫面" /><br/><br/>
                                    <asp:LinkButton ID="LinkButton1" CssClass="text-red" OnClientClick="OpenD351()" runat="server">決標公告(一般)稿預覽列印</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <footer class="panel-footer" style="text-align: center;">
                        <!--網頁尾-->
                    </footer>
                </section>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
