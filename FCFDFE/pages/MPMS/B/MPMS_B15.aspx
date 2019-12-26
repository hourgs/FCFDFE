<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MPMS_B15.aspx.cs" Inherits="FCFDFE.pages.MPMS.B.MPMS_B15" %>

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
                    購案轉呈計劃評核單位
                </header>
                <asp:Panel ID="PnMessage" runat="server"></asp:Panel>
                <!--預留空間，未來做錯誤訊息顯示。-->
                <div class="panel-body" style="border: solid 2px;">
                    <div class="form" style="border: 5px;">
                        <div class="cmxform form-horizontal tasi-form">
                        <table class="table table-bordered text-center">
                            <tr>
                                <td style="width: 35%;">
                                    <asp:Label CssClass="control-label" runat="server">購案編號</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:TextBox ID="txtOVC_PURCH" CssClass="tb tb-l" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnQuery" CssClass="btn-success btnw4" runat="server" Text="查詢" OnClick="btnQuery_Click"/><!--綠色-->
                                    <asp:Button ID="btnReset" CssClass="btn-default btnw4" runat="server" Text="清除" OnClick="btnReset_Click"/><!--灰色-->
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">採購單位地區</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblOVC_PUR_AGENCY" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">購案名稱</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblOVC_PUR_IPURCH" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">申辦單位(代碼)-申購人(電話)</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblOVC_PUR_NSECTION" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="lblBrackets_1" CssClass="control-label" runat="server" Text="("></asp:Label>
                                    <asp:Label ID="lblOVC_PUR_SECTION" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="lblBrackets_2" CssClass="control-label" runat="server" Text=")"></asp:Label>
                                    <asp:Label ID="lblConnect" CssClass="control-label" runat="server" Text="-"></asp:Label>
                                    <asp:Label ID="lblOVC_PUR_USER" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="lblBrackets_3" CssClass="control-label" runat="server" Text="("></asp:Label>
                                    <asp:Label ID="lblOVC_PUR_IUSER_PHONE_EXT" CssClass="control-label" runat="server"></asp:Label>
                                    <asp:Label ID="lblBrackets_4" CssClass="control-label" runat="server" Text=")"></asp:Label><%--1301PLAN  OVC_PUR_IUSER_PHONE_EXT--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">決標原則</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblOVC_BID" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">購報價及決標方式</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblOVC_BID_METHOD_1" CssClass="control-label" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">投標段次</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblOVC_BID_TIMES" CssClass="control-label" runat="server">不分段開標</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">採購屬性(採購性質)
                                    </asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblOVC_LAB" CssClass="control-label" runat="server">勞務採標</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">招標方式<br />保留增購金額</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblOVC_PUR_ASS_VEN_CODE" CssClass="control-label" runat="server">公開招標<br />無</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">押標金額度<br />特殊採購(臨停、緊急、臨時緊急)</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:Label ID="lblOVC_BID_MONEY" CssClass="control-label" runat="server">不少於投標文件標價百分之00</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label position-left" runat="server">協商措施</asp:Label><br />
                                    <asp:Label CssClass="control-label position-left" runat="server">申請日期：</asp:Label><!--前方標籤文字，跟日期同一行需使用"position-left"之class-->
                                    <!--↓日期套件↓-->
                                    <div class="input-append datepicker position-left" data-date="<%=DateTime.Now%>" data-date-format="yyyy-mm-dd" data-date-viewmode="years">
                                        <asp:TextBox ID="txtOVC_DAPPLY" CssClass="tb tb-s position-left" runat="server"></asp:TextBox>
                                        <div class="add-on"><i class="icon-calendar"></i></div>
                                    </div>
                                    <!--↑日期套件↑-->
                                </td>
                                <td class="text-left">
                                    <asp:Label CssClass="control-label" runat="server">申請文號：</asp:Label><asp:TextBox ID="txtOVC_PROPOSE" CssClass="tb tb-s" runat="server"></asp:TextBox><br />
                                    <asp:Label ID="lblOVC_PROPOSE" CssClass="control-label" runat="server">申請文號請輸入如[刻則字第0950000001號]之文號，請勿在後面輸入令、凾等字</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">申請單位主管(含級職及姓名)</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:TextBox ID="txtOVC_APPLY_CHIEF" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">計劃評核(審查)單位</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:TextBox id="txtOVC_TO_UNIT_NAME" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtOVC_AUDIT_UNIT"  runat="server" Forecolor="Red" ErrorMessage="必填"></asp:RequiredFieldValidator>
                                    <asp:TextBox id="txtOVC_TO_UNIT_NAME_1" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnTO_UNIT" OnClientClick="OpenWindow('txtOVC_TO_UNIT_NAME','txtOVC_TO_UNIT_NAME_1')" cssclass="btn-warning" runat="server" Text="單位查詢"/><br />
                                    <%--<asp:TextBox ID="txtOVC_CHECK_UNIT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:DropDownList ID="drpOVC_CHECK_UNIT" CssClass="tb tb-l" runat="server">
                                        <asp:ListItem>00N00國防部國防採購室</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Button ID="btnQuery_department" CssClass="btn-success btnw4" runat="server" Text="單位查詢" /><!--綠色--><br />--%>
                                    <asp:Label ID="lblinputup" CssClass="control-label" runat="server">(請輸入上一級之計劃評核單位)</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">最後計劃評核(審查)單位</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:TextBox id="txtOVC_AUDIT_UNIT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valOVC_AUDIT_UNIT" ControlToValidate="txtOVC_AUDIT_UNIT"  runat="server" Forecolor="Red" ErrorMessage="必填"></asp:RequiredFieldValidator>
                                    <asp:TextBox id="txtOVC_AUDIT_UNIT_1" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnAudit" onclick="btnAudit_Click" OnClientClick="OpenWindow('txtOVC_AUDIT_UNIT','txtOVC_AUDIT_UNIT_1')" cssclass="btn-warning" runat="server" Text="單位查詢"/><br />
                                    <asp:Label ID="lblFINAL" CssClass="control-label" runat="server">(最後的計劃評核(審查)單位為採購預劃之計劃評核單位，如果採購預劃輸入錯誤請再此更改，將寫回採購預劃之計劃評核單位)</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">採購發包單位</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:TextBox id="txtOVC_PURCHASE_UNIT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valOVC_PURCHASE_UNIT" ControlToValidate="txtOVC_PURCHASE_UNIT"  runat="server" Forecolor="Red" ErrorMessage="必填"></asp:RequiredFieldValidator>
                                    <asp:TextBox id="txtOVC_PURCHASE_UNIT_1" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnQueryOVC_PURCHASE_UNIT" onclick="btnQueryOVC_PURCHASE_UNIT_Click" OnClientClick="OpenWindow('txtOVC_PURCHASE_UNIT','txtOVC_PURCHASE_UNIT_1')" cssclass="btn-warning" runat="server" Text="單位查詢"/><br />
                                    <asp:Label ID="lblOVC_PURCHASE_UNIT" CssClass="control-label" runat="server">(採欄位為採購預劃之採購發包單位，如果採購預劃輸入錯誤將在此更改，將寫回採購預劃之採購發包單位)</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label CssClass="control-label" runat="server">履約驗結單位單位</asp:Label>
                                </td>
                                <td class="text-left">
                                    <asp:TextBox id="txtOVC_CONTRACT_UNIT" CssClass="tb tb-s" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valOVC_CONTRACT_UNIT" ControlToValidate="txtOVC_CONTRACT_UNIT"  runat="server" Forecolor="Red" ErrorMessage="必填"></asp:RequiredFieldValidator>
                                    <asp:TextBox id="txtOVC_CONTRACT_UNIT_1" CssClass="tb tb-m" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnQueryOVC_CONTRACT_UNIT" onclick="btnQueryOVC_CONTRACT_UNIT_Click" OnClientClick="OpenWindow('txtOVC_CONTRACT_UNIT','txtOVC_CONTRACT_UNIT_1')" cssclass="btn-warning" runat="server" Text="單位查詢"/><br />
                                    <asp:Label ID="lblCONTRACT" CssClass="control-label" runat="server">(若仍由採購室下委辦單位履約者，仍請選定採購室為履約驗結單位)</asp:Label><br />
                                    <asp:Label ID="lblCONTRACT_CHANGE" CssClass="control-label" runat="server">(此欄位為採購預劃之履約驗結單位，如果採購預劃輸入請再此更改，將寫回採購預劃之履約驗結單位)</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <h4>確定轉呈後，即無法在異動購案資料(除非上級單位退回或第一次申辦而上級單位尚未分案)</h4>
                                </td>
                            </tr>
                        </table>
                            
                        <div class="text-center">
                            <asp:Button ID="btnSave" CssClass="btn-warning btnw2" runat="server" Text="確認" OnClick="btnSave_Click"/><!--黃色-->
                        </div>

                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
